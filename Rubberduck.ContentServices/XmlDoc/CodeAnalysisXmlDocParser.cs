using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using RubberduckServices.Abstract;
using Rubberduck.ContentServices.XmlDoc.Schema;
using Rubberduck.ContentServices.XmlDoc.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.ContentServices.Model;
using Rubberduck.Model;
using System.Collections.Concurrent;

namespace Rubberduck.ContentServices.XmlDoc
{
    public class CodeAnalysisXmlDocParser : XmlDocParserBase, ICodeAnalysisXmlDocParser
    {
        private readonly IContentService _content;
        private readonly ISyntaxHighlighterService _syntaxHighlighterService;
        private readonly IDictionary<string, InspectionDefaultConfig> _inspectionDefaults;

        public CodeAnalysisXmlDocParser(IContentService content,
            ISyntaxHighlighterService syntaxHighlighterService, 
            IEnumerable<InspectionDefaultConfig> inspectionDefaults)
            : base("Rubberduck.CodeAnalysis.xml")
        {
            _content = content;
            _syntaxHighlighterService = syntaxHighlighterService;
            _inspectionDefaults = inspectionDefaults.ToDictionary(e => e.InspectionName, e => e);
        }

        protected override async Task<IEnumerable<FeatureItem>> ParseAsync(int assetId, XDocument document, bool isPreRelease)
        {
            var quickfixesFeatureId = (await _content.GetFeatureAsync("QuickFixes"))?.Id
                ?? throw new InvalidOperationException("Could not retrieve a FeatureId for the 'QuickFixes' feature.");
            var quickFixes = ReadQuickFixes(assetId, quickfixesFeatureId, document, !isPreRelease);

            var inspectionsFeatureId = (await _content.GetFeatureAsync("Inspections"))?.Id
                ?? throw new InvalidOperationException("Could not retrieve a FeatureId for the 'Inspections' feature.");
            var inspections = ReadInspections(assetId, inspectionsFeatureId, document, !isPreRelease, quickFixes);

            return inspections.Concat(quickFixes);
        }

        private IEnumerable<FeatureItem> ReadInspections(int assetId, int featureId, XDocument doc, bool hasReleased, IEnumerable<FeatureItem> quickFixes)
        {
            var nodes = from node in doc.Descendants("member").AsParallel()
                        let name = GetInspectionNameOrDefault(node)
                        where !string.IsNullOrWhiteSpace(name)
                        let inspectionName = name.Substring(name.LastIndexOf(".", StringComparison.Ordinal) + 1)
                        let config = _inspectionDefaults.ContainsKey(inspectionName) ? _inspectionDefaults[inspectionName] : default
                        select (name, node, config);

            var results = new ConcurrentBag<FeatureItem>();
            Parallel.ForEach(nodes, info =>
            {
                var xmldoc = new XmlDocInspection(_syntaxHighlighterService, info.name, info.node, info.config, !hasReleased);
                results.Add(xmldoc.Parse(assetId, featureId, quickFixes));
            });

            return results;
        }

        private static string GetInspectionNameOrDefault(XElement memberNode)
        {
            var name = memberNode.Attribute("name")?.Value;
            if (name == null || !name.StartsWith("T:") || !name.EndsWith("Inspection") || name.EndsWith("IInspection"))
            {
                return default;
            }

            return name;
        }

        private IEnumerable<FeatureItem> ReadQuickFixes(int assetId, int featureId, XDocument doc, bool hasReleased)
        {
            var nodes = from node in doc.Descendants("member").AsParallel()
                        let name = GetQuickFixNameOrDefault(node)
                        where !string.IsNullOrEmpty(name) && node.Descendants(XmlDocSchema.QuickFix.CanFix.ElementName).Any() // this excludes any quickfixes added to main (master) prior to v2.5.0 
                        select (name, node);
            //select .Parse(assetId, featureId);
            var results = new ConcurrentBag<FeatureItem>();
            Parallel.ForEach(nodes, info =>
            {
                var xmldoc = new XmlDocQuickFix(_syntaxHighlighterService, info.name, info.node, !hasReleased);
                results.Add(xmldoc.Parse(assetId, featureId));
            });

            return results;
        }

        private static string GetQuickFixNameOrDefault(XElement memberNode)
        {
            var name = memberNode.Attribute("name")?.Value;
            if (name == null || !name.StartsWith("T:") || !name.EndsWith("QuickFix"))
            {
                return default;
            }

            return name;
        }
    }
}
