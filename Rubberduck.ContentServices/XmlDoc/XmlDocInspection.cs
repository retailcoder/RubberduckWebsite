using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Rubberduck.ContentServices.Model;
using Rubberduck.ContentServices.XmlDoc.Schema;
using RubberduckServices.Abstract;
using Rubberduck.Model;
using PublicModel = Rubberduck.Model.Entities;

namespace Rubberduck.ContentServices.XmlDoc
{
    public class XmlDocInspection
    {
        private static readonly string _defaultSeverity = "Warning";
        private static readonly string _defaultInspectionType = "CodeQualityIssues";

        public XmlDocInspection(ISyntaxHighlighterService service, string name, XElement node, InspectionDefaultConfig config, bool isPreRelease)
        {
            SyntaxHighlighterService = service;

            SourceObject = name;
            TypeName = name.Substring(name.LastIndexOf(".", StringComparison.Ordinal) + 1);

            IsPreRelease = isPreRelease;
            InspectionName = TypeName.Replace("Inspection", string.Empty).Trim();

            Summary = node.Element(XmlDocSchema.Inspection.Summary.ElementName)?.Value.Trim();
            IsHidden = node.Element(XmlDocSchema.Inspection.Summary.ElementName)?.Attribute(XmlDocSchema.Inspection.Summary.IsHiddenAttribute)?.Value.Equals(true.ToString(), StringComparison.InvariantCultureIgnoreCase) ?? false;
            Reasoning = node.Element(XmlDocSchema.Inspection.Reasoning.ElementName)?.Value.Trim();
            References = node.Elements(XmlDocSchema.Inspection.Reference.ElementName).Select(e => e.Attribute(XmlDocSchema.Inspection.Reference.NameAttribute)?.Value.Trim()).ToArray();
            HostApp = node.Element(XmlDocSchema.Inspection.HostApp.ElementName)?.Attribute(XmlDocSchema.Inspection.HostApp.NameAttribute)?.Value.Trim();
            Remarks = node.Element(XmlDocSchema.Inspection.Remarks.ElementName)?.Value;

            DefaultSeverity = config?.DefaultSeverity ?? _defaultSeverity;
            InspectionType = config?.InspectionType ?? _defaultInspectionType;

            Examples = ParseExamples(node).ToArray();
        }

        public string SourceObject { get; }
        public bool IsPreRelease { get; }

        public bool IsHidden { get; }
        public string TypeName { get; }
        public string InspectionName { get; }
        public string Summary { get; }
        public string[] References { get; }
        public string HostApp { get; }
        public string Reasoning { get; }
        public string Remarks { get; }
        public string InspectionType { get; }
        public string DefaultSeverity { get; }

        public Example[] Examples { get; }

        public FeatureItem Parse(int assetId, int featureId, IEnumerable<FeatureItem> quickFixes)
        {
            var dto = new FeatureItem
            {
                FeatureId = featureId,
                Name = InspectionName,
                IsHidden = IsHidden,
                IsNew = IsPreRelease,
                Title = InspectionName,
                Description = Reasoning,
                TagAssetId = assetId,
                XmlDocSummary = Summary,
                XmlDocInfo = string.Join(",", quickFixes.Where(fix => fix.XmlDocMetadata?.Contains(InspectionName, StringComparison.InvariantCultureIgnoreCase) ?? fix.Name == "IgnoreOnce").Select(fix => fix.Name)),
                XmlDocRemarks = Remarks,
                XmlDocSourceObject = SourceObject,
                XmlDocTabName = InspectionType,
                XmlDocMetadata = DefaultSeverity,
            };

            // TODO move HTML fragments?
            if (!string.IsNullOrEmpty(HostApp))
            {
                dto.XmlDocInfo += $"<p id=\"host_or_library_specific_info\"><span class=\"icon icon-info\"></span>This inspection will only run if the host application is <code>{HostApp}</code>.</p>";
            }
            else if (References.Length == 1)
            {
                dto.XmlDocInfo += $"<p id=\"host_or_library_specific_info\"><span class=\"icon icon-info\"></span>This inspection will only run if the <code>{References[0]}</code> library is referenced.</p>";
            }
            else if (References.Length > 1)
            {
                var libraries = string.Join(", ", References.Select(lib => $"<code>{lib}</code>"));
                dto.XmlDocInfo += $"<p id=\"host_or_library_specific_info\"><span class=\"icon icon-info\"></span>This inspection will only run if one or a combination of the following libraries is referenced: {libraries}</p>";
            }

            if (quickFixes?.Any() ?? false)
            {
                var sorted = quickFixes.OrderBy(fix => fix.Name.StartsWith("IgnoreOnce") ? "__0" : fix.Name);
                var fixes = string.Join(" ", sorted.Select(fix => $"<li>{(fix.Name.StartsWith("IgnoreOnce") ? "<span class=\"icon icon-ignoreonce\"></span>" : "<span class=\"icon icon-tick\"></span>")}<a href=\"https://rubberduckvba.com/QuickFixes/Details/{fix.Name}\">{fix.Name}</a>: {fix.XmlDocSummary}</li>"));
                dto.XmlDocInfo += $"<div><h5>Quick-Fixes</h5><p>The following quick-fixes are available for this inspection:</p><ul style=\"margin-left: 8px; list-style: none;\">{fixes}</ul></div>";
            }

            dto.Examples = Examples;
            return dto;
        }

        public ISyntaxHighlighterService SyntaxHighlighterService { get; }

        private IEnumerable<Example> ParseExamples(XElement node)
        {
            var moduleTypes = typeof(PublicModel.ExampleModuleType).GetMembers()
                .Select(m => (m.Name, m.GetCustomAttributes().OfType<System.ComponentModel.DescriptionAttribute>().SingleOrDefault()?.Description))
                .Where(m => m.Description != null)
                .ToDictionary(m => m.Description, m => (PublicModel.ExampleModuleType)Enum.Parse(typeof(PublicModel.ExampleModuleType), m.Name, true));

            return node.Elements(XmlDocSchema.Inspection.Example.ElementName)
                .Select((e, i) =>
                    new Example
                    {
                        Description = (e.Attribute(XmlDocSchema.Inspection.Example.HasResultAttribute)?.Value.Equals(true.ToString(), StringComparison.InvariantCultureIgnoreCase) ?? true)
                            ? "<span class=\"icon icon-inspection\"></span>The following code <em>should</em> trigger this inspection:"
                            : "<span class=\"icon icon-tick\"></span>The following code should <strong>NOT</strong> trigger this inspection:",
                        SortOrder = i,
                        Modules = e.Elements(XmlDocSchema.Inspection.Example.Module.ElementName)
                            .Select(m =>
                                new ExampleModule
                                {
                                    HtmlContent = SyntaxHighlighterService.FormatAsync(m.Nodes().OfType<XCData>().Single().Value).ConfigureAwait(false).GetAwaiter().GetResult(),
                                    ModuleName = m.Attribute(XmlDocSchema.Inspection.Example.Module.ModuleNameAttribute)?.Value,
                                    ModuleTypeId = (int)(moduleTypes.TryGetValue(m.Attribute(XmlDocSchema.Inspection.Example.Module.ModuleTypeAttribute).Value, out var type) ? type : PublicModel.ExampleModuleType.Any)
                                })
                            .Concat(e.Nodes().OfType<XCData>().Select(x =>
                                new ExampleModule
                                {
                                    HtmlContent = SyntaxHighlighterService.FormatAsync(x.Value).ConfigureAwait(false).GetAwaiter().GetResult(),
                                    ModuleName = "Module1",
                                    ModuleTypeId = (int)PublicModel.ExampleModuleType.Any
                                }).Take(1)).ToList()
                    });
        }
    }
}
