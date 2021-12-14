using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Rubberduck.Model.Internal;
using RubberduckServices.Abstract;

namespace Rubberduck.ContentServices.XmlDoc
{
    public class XmlDocAnnotation
    {
        public XmlDocAnnotation(ISyntaxHighlighterService service, string name, XElement node, bool isPreRelease)
        {
            SyntaxHighlighterService = service;

            SourceObject = name;
            IsPreRelease = isPreRelease;

            AnnotationName = name.Substring(name.LastIndexOf(".", StringComparison.Ordinal) + 1).Replace("Annotation", string.Empty);
            Summary = node.Element(XmlDocSchema.Annotation.Summary.ElementName)?.Value.Trim();
            Remarks = node.Element(XmlDocSchema.Annotation.Remarks.ElementName)?.Value;

            Parameters = node.Elements(XmlDocSchema.Annotation.Parameter.ElementName)
                .Select(e => (Name: node.Attribute(XmlDocSchema.Annotation.Parameter.NameAttribute)?.Value ?? string.Empty,
                              Type: node.Attribute(XmlDocSchema.Annotation.Parameter.TypeAttribute)?.Value ?? string.Empty,
                              Description: node.Value))
                .Select(e => new AnnotationArgInfo(e.Name, e.Type, e.Description))
                .ToArray();

            Examples = ParseExamples(node).ToArray();
        }

        public string SourceObject { get; }
        public bool IsPreRelease { get; }

        public string AnnotationName { get; }
        public string Summary { get; }
        public string Remarks { get; }

        public IReadOnlyList<AnnotationArgInfo> Parameters { get; }
        public IReadOnlyList<BeforeAndAfterCodeExample> Examples { get; }

        public ISyntaxHighlighterService SyntaxHighlighterService { get; }

        public FeatureItem Parse(int assetId)
        {
            var parameters = string.Join(string.Empty, Parameters.Select(p => $"<tr><td>{p.Name}</td><td>{p.Type}</td><td>{p.Description}</td></tr>"));
            var parameterInfo = Parameters.Count == 0 ? string.Empty
                : $"<table class=\"parameters-table\"><caption>Parameters</caption><thead><tr><th>Name</th><th>Type</th><th>Description</th></tr></thead>{parameters}</table>";

            var dto = new Model.DTO.FeatureItem
            {
                Name = AnnotationName,
                IsNew = IsPreRelease,
                Title = Summary,
                Description = Remarks,
                TagAssetId = assetId,
                XmlDocSummary = Summary,
                XmlDocInfo = parameterInfo,
                XmlDocRemarks = Remarks,
                XmlDocSourceObject = SourceObject,
                XmlDocTabName = null,
                XmlDocMetadata = null,
            };

            var examples = Examples.Select((e, i) => e.AsExample(string.Empty, i));
            return FeatureItem.FromDTO(dto, examples);
        }

        private BeforeAndAfterCodeExample[] ParseExamples(XElement node)
        {
            try
            {
                var examples = new List<BeforeAndAfterCodeExample>();
                var exampleNodes = node.Elements(XmlDocSchema.Annotation.Example.ElementName);
                foreach (var example in exampleNodes)
                {
                    /* <example>
                     *   <module><![CDATA[
                     *   'VBA CODE
                     *   ]]>
                     *   </module>
                     * </example>
                    */
                    var modules = example.Elements(XmlDocSchema.Annotation.Example.Module.ElementName);
                    var simpleExamples = modules.Where(m => m.Nodes().OfType<XCData>().Any())
                        .Select(e => new BeforeAndAfterCodeExample(new[] { FormatCodeExample(e) }, modulesAfter: null))
                        .ToArray();
                    if (simpleExamples.Length > 0)
                    {
                        examples.AddRange(simpleExamples.ToArray());
                        continue;
                    }

                    IEnumerable<ExampleModule> before = Enumerable.Empty<ExampleModule>();
                    IEnumerable<ExampleModule> after = null;

                    if (modules.Any())
                    {
                        /* <example>
                         *   <module>
                         *     <before><![CDATA[
                         *   'VBA CODE
                         *   ]]>
                         *     </before>
                         *     <after><![CDATA[
                         *   'VBA CODE
                         *   ]]>
                         *     </after>
                         *   </module>
                         * </example>
                        */
                        before = modules.Select(e => FormatCodeExample(e.Element(XmlDocSchema.Annotation.Example.Module.Before.ElementName), "(code pane)"));
                        after = modules.Select(e => FormatCodeExample(e.Element(XmlDocSchema.Annotation.Example.Module.After.ElementName), "(synchronized, hidden attributes shown)"));
                    }

                    if (example.Elements(XmlDocSchema.Annotation.Example.Before.ElementName).Any())
                    {
                        /* <example>
                         *   <before>
                         *     <module><![CDATA[
                         *   'VBA CODE
                         *   ]]>
                         *     </module>
                         *   </before>
                         *   <after>
                         *     <module><![CDATA[
                         *   'VBA CODE
                         *   ]]>
                         *     </module>
                         *   </after>
                         * </example>
                        */
                        before = example.Elements(XmlDocSchema.Annotation.Example.Before.ElementName)
                            .Select(e => FormatCodeExample(e.Element(XmlDocSchema.Annotation.Example.Before.Module.ElementName), "(code pane)"));
                        after = example.Elements(XmlDocSchema.Annotation.Example.After.ElementName)
                            .Select(e => FormatCodeExample(e.Element(XmlDocSchema.Annotation.Example.After.Module.ElementName), "(synchronized, hidden attributes shown)"));
                    }

                    if (before.Any() && after.Any())
                    {
                        examples.Add(new BeforeAndAfterCodeExample(before, after));
                    }
                }
                return examples.ToArray();
            }
            catch (Exception)
            {
                var errorExample = new[] { Model.DTO.ExampleModule.ParseError("AnnotationExample") }.Select(ExampleModule.FromDTO);
                return new[] { new BeforeAndAfterCodeExample(errorExample, errorExample) };
            }
        }

        private static readonly IDictionary<string, ExampleModuleType> ModuleTypes = typeof(ExampleModuleType).GetMembers()
            .Select(m => (m.Name, m.GetCustomAttributes().OfType<System.ComponentModel.DescriptionAttribute>().Single().Description))
            .ToDictionary(m => m.Description, m => (ExampleModuleType)Enum.Parse(typeof(ExampleModuleType), m.Name, true));

        private ExampleModule FormatCodeExample(XElement cdataParent, string description = null)
        {
            var module = cdataParent.AncestorsAndSelf(XmlDocSchema.Annotation.Example.Module.ElementName).Single();
            var name = module.Attribute(XmlDocSchema.Annotation.Example.Module.ModuleNameAttribute)?.Value;
            var moduleType = (int)(ModuleTypes.TryGetValue(module.Attribute(XmlDocSchema.Annotation.Example.Module.ModuleTypeAttribute)?.Value, out var type) ? type : ExampleModuleType.Any);
            var code = SyntaxHighlighterService.FormatAsync(cdataParent.Nodes().OfType<XCData>().Single().Value).ConfigureAwait(false).GetAwaiter().GetResult();
            
            var dto = new Model.DTO.ExampleModule
            {
                ModuleName = name,
                ModuleType = moduleType,
                Description = description,
                HtmlContent = code
            };

            return ExampleModule.FromDTO(dto);
        }
    }

    public class AnnotationArgInfo
    {
        public AnnotationArgInfo(string name, string type, string description)
        {
            Name = name;
            Type = type;
            Description = description;
        }

        public string Name { get; }
        public string Type { get; }
        public string Description { get; }
    }
}
