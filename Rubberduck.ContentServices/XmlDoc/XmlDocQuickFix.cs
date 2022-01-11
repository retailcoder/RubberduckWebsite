using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Rubberduck.ContentServices.Model;
using Rubberduck.ContentServices.XmlDoc.Schema;
using PublicModel = Rubberduck.Model.Entities;

namespace Rubberduck.ContentServices.XmlDoc
{
    public class XmlDocQuickFix : IEquatable<XmlDocQuickFix>
    {
        public XmlDocQuickFix(string name, XElement node, bool isPreRelease)
        {
            SourceObject = name;
            QuickFixName = name.Substring(name.LastIndexOf(".", StringComparison.Ordinal) + 1).Replace("QuickFix", string.Empty).Trim();
            Summary = node.Element(XmlDocSchema.QuickFix.Summary.ElementName)?.Value.Trim();
            Remarks = node.Element(XmlDocSchema.QuickFix.Remarks.ElementName)?.Value.Trim();
            IsPreRelease = isPreRelease;

            var canFixNode = node.Element(XmlDocSchema.QuickFix.CanFix.ElementName);
            CanFixInProcedure = Convert.ToBoolean(canFixNode?.Attribute(XmlDocSchema.QuickFix.CanFix.ProcedureAttribute)?.Value ?? true.ToString());
            CanFixInModule = Convert.ToBoolean(canFixNode?.Attribute(XmlDocSchema.QuickFix.CanFix.ModuleAttribute)?.Value ?? true.ToString());
            CanFixInProject = Convert.ToBoolean(canFixNode?.Attribute(XmlDocSchema.QuickFix.CanFix.ProjectAttribute)?.Value ?? true.ToString());

            Inspections = node.Element(XmlDocSchema.QuickFix.Inspections.ElementName)?
                .Elements(XmlDocSchema.QuickFix.Inspections.Inspection.ElementName)
                .Select(e => e.Attribute(XmlDocSchema.QuickFix.Inspections.Inspection.NameAttribute)?.Value)
                .ToArray();

            Examples = ParseExamples(node).ToArray();
        }

        public FeatureItem Parse(int assetId, int featureId)
        {
            var applicableScopes = string.Join("", new[]
                {
                    CanFixInProcedure ? "<span class=\"icon icon-procedure\"></span>Procedure" : null,
                    CanFixInModule ? "<span class=\"icon icon-standard-module\"></span>Module" : null,
                    CanFixInProject ? "<span class=\"icon icon-project\"></span>Project" : null
                }
                .Where(scope => scope != null)
                .Select(scope => $"<p style=\"margin-top: 10px;\"><li>{scope}</li></p>"));
            string scopeInfo = string.Empty;
            if (applicableScopes.Any())
            {
                scopeInfo = $"This quickfix addresses the selected inspection result, but can also be applied to all similar inspection results in the following scopes (as a single operation):<ul>{applicableScopes}</ul>";
            }
            else
            {
                scopeInfo = $"This quickfix can only be applied to a single inspection result at once.";
            }

            string inspectionInfo = null;
            if (Inspections?.Count() == 1)
            {
                inspectionInfo = Inspections.Single();
            }
            else if (Inspections?.Any() ?? false)
            {
                inspectionInfo = string.Join(",", Inspections);
            }

            var dto = new FeatureItem
            {
                FeatureId = featureId,

                Name = QuickFixName,
                IsHidden = false,
                IsNew = IsPreRelease,
                Title = QuickFixName,
                Description = scopeInfo,
                TagAssetId = assetId,
                XmlDocSummary = Summary,
                XmlDocInfo = null,
                XmlDocRemarks = Remarks,
                XmlDocSourceObject = SourceObject,
                XmlDocTabName = null,
                XmlDocMetadata = inspectionInfo,

                Examples = Examples.Select((e, i) => e.AsExample(string.Empty, i)).ToList()
            };

            return dto;
        }

        public IEnumerable<string> Inspections { get; } = Enumerable.Empty<string>();

        public string SourceObject { get; }

        public string QuickFixName { get; }

        public string Summary { get; }
        public string Remarks { get; }
        public bool IsPreRelease { get; }

        public bool CanFixInProcedure { get; }
        public bool CanFixInModule { get; }
        public bool CanFixInProject { get; }

        public BeforeAndAfterCodeExample[] Examples { get; }

        public bool Equals(XmlDocQuickFix other) => other.QuickFixName == QuickFixName;

        public override bool Equals(object obj) => Equals((XmlDocQuickFix)obj);
        public override int GetHashCode() => QuickFixName.GetHashCode();

        private static readonly IDictionary<string, PublicModel.ExampleModuleType> ModuleTypes =
            typeof(PublicModel.ExampleModuleType)
                .GetMembers()
                .Select(m => (m.Name, m.GetCustomAttributes().OfType<System.ComponentModel.DescriptionAttribute>().SingleOrDefault()?.Description))
                .Where(m => m.Description != null)
                .ToDictionary(m => m.Description, m => (PublicModel.ExampleModuleType)Enum.Parse(typeof(PublicModel.ExampleModuleType), m.Name, true));

        private IEnumerable<BeforeAndAfterCodeExample> ParseExamples(XElement node)
        {
            var results = new List<BeforeAndAfterCodeExample>();
            foreach (var exampleNode in node.Elements(XmlDocSchema.QuickFix.Example.ElementName))
            {
                var before = exampleNode.Element(XmlDocSchema.QuickFix.Example.Before.ElementName)?
                    .Elements(XmlDocSchema.QuickFix.Example.Before.Module.ElementName)?.Select(m =>
                        new ExampleModule
                        {
                            ModuleName = m.Attribute(XmlDocSchema.QuickFix.Example.Before.Module.ModuleNameAttribute)?.Value,
                            Description = "(before)",
                            ModuleTypeId = (int)(ModuleTypes.TryGetValue(m.Attribute(XmlDocSchema.QuickFix.Example.Before.Module.ModuleTypeAttribute).Value, out var type) ? type : PublicModel.ExampleModuleType.Any),
                            HtmlContent = m.Nodes().OfType<XCData>().Single().Value
                        })
                    .Concat(exampleNode.Element(XmlDocSchema.QuickFix.Example.Before.ElementName)?.Nodes().OfType<XCData>().Take(1).Select(x =>
                        new ExampleModule
                        {
                            ModuleName = "Module1",
                            Description = "(before)",
                            ModuleTypeId = (int)PublicModel.ExampleModuleType.Any,
                            HtmlContent = x.Value
                        }));

                var after = exampleNode.Element(XmlDocSchema.QuickFix.Example.After.ElementName)?
                    .Elements(XmlDocSchema.QuickFix.Example.After.Module.ElementName)?.Select(m =>
                        new ExampleModule
                        {
                            ModuleName = m.Attribute(XmlDocSchema.QuickFix.Example.After.Module.ModuleNameAttribute)?.Value,
                            Description = "(after)",
                            ModuleTypeId = (int)(ModuleTypes.TryGetValue(m.Attribute(XmlDocSchema.QuickFix.Example.After.Module.ModuleTypeAttribute).Value, out var type) ? type : PublicModel.ExampleModuleType.Any),
                            HtmlContent = m.Nodes().OfType<XCData>().Single().Value
                        })
                    .Concat(exampleNode.Element(XmlDocSchema.QuickFix.Example.After.ElementName)?.Nodes().OfType<XCData>().Take(1).Select(x =>
                        new ExampleModule
                        {
                            ModuleName = "Module1",
                            Description = "(after)",
                            ModuleTypeId = (int)PublicModel.ExampleModuleType.Any,
                            HtmlContent = x.Value
                        }));
                results.Add(new BeforeAndAfterCodeExample(before, after));
            }
            return results;
        }
    }
}
