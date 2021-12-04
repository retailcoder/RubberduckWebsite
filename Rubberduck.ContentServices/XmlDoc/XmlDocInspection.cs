using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Rubberduck.ContentServices.XmlDoc.Abstract;
using Rubberduck.Model.Entity;

namespace Rubberduck.ContentServices.XmlDoc
{
    public class InspectionXmlDocParser : IInspectionXmlDocParser
    {
        public Task<FeatureItem> ParseAsync()
        {
            throw new NotImplementedException();
        }
    }

    public class XmlDocInspection
    {
        private static readonly string _defaultSeverity = "Warning";
        private static readonly string _defaultInspectionType = "CodeQualityIssues";

        public XmlDocInspection(string name, XElement node, XElement config, bool isPreRelease)
        {
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

            DefaultSeverity = config.Attribute(XmlDocSchema.Inspection.Config.SeverityAttribute)?.Value.Trim() ?? _defaultSeverity;
            InspectionType = config.Attribute(XmlDocSchema.Inspection.Config.InspectionTypeAttribute)?.Value.Trim() ?? _defaultInspectionType;
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

        public async Task<FeatureItem> ParseAsync()
        {
            var dto = new Model.DTO.FeatureItem
            {
                Name = InspectionName,
                IsHidden = IsHidden,
                IsNew = IsPreRelease,
                Title = Summary,
                Description = Reasoning,
                XmlDocSummary = Summary,
                XmlDocInfo = Reasoning,
                XmlDocRemarks = Remarks,
                XmlDocSourceObject = SourceObject,
                XmlDocTabName = InspectionType,
                XmlDocMetadata = DefaultSeverity,
            };

            // TODO move HTML fragments to config db table?
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

            /* TODO
            if (QuickFixes.Any())
            {
                var sorted = QuickFixes.OrderBy(fix => fix.QuickFixName.StartsWith("IgnoreOnce") ? "__0" : fix.QuickFixName);
                var fixes = string.Join(" ", sorted.Select(fix => $"<li>{(fix.QuickFixName.StartsWith("IgnoreOnce") ? "<span class=\"icon icon-ignoreonce\"></span>" : "<span class=\"icon icon-tick\"></span>")}<a href=\"https://rubberduckvba.com/QuickFixes/Details/{fix.QuickFixName}\">{fix.QuickFixName}</a>: {fix.Summary}</li>"));
                dto.XmlDocInfo += $"<div><h5>Quick-Fixes</h5><p>The following quick-fixes are available for this inspection:</p><ul style=\"margin-left: 8px; list-style: none;\">{fixes}</ul></div>";
            }
            */

            var examples = Enumerable.Empty<Example>(); // TODO
            return FeatureItem.FromDTO(dto, examples);
        }
    }

    public class XmlDocQuickFix
    {

    }

    public class XmlDocAnnotation
    {

    }
}
