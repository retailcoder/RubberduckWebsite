using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Rubberduck.ContentServices.XmlDoc.Abstract;
using Rubberduck.Model.Internal;
using Rubberduck.Model.ViewModel;
using RubberduckServices.Abstract;

namespace Rubberduck.ContentServices.XmlDoc
{
    public class CodeAnalysisXmlDocParser : ICodeAnalysisXmlDocParser
    {
        private readonly ISyntaxHighlighterService _syntaxHighlighterService;
        private readonly IDictionary<string, InspectionDefaultConfig> _inspectionDefaults;

        public CodeAnalysisXmlDocParser(ISyntaxHighlighterService syntaxHighlighterService, IEnumerable<InspectionDefaultConfig> inspectionDefaults)
        {
            _syntaxHighlighterService = syntaxHighlighterService;
            _inspectionDefaults = inspectionDefaults.ToDictionary(e => e.InspectionName, e => e);
        }

        public string AssetName { get; } = "Rubberduck.CodeAnalysis";

        public async Task<IEnumerable<FeatureItem>> ParseAsync(Tag tag)
        {
            var asset = tag.Assets.SingleOrDefault(a => a.Name.Contains(AssetName))
                ?? throw new InvalidOperationException($"Asset '{AssetName}' was not found under the specified tag.");

            var uri = asset.DownloadUrl;
            if (uri.Host != "github.com")
            {
                throw new UriFormatException($"Unexpected host in download URL '{uri}' from asset ID {asset.Id} (tag ID {tag.Id}, '{tag.Name}').");
            }

            using (var client = new HttpClient())
            using (var response = await client.GetAsync(uri))
            {
                if (response.IsSuccessStatusCode)
                {
                    using var stream = await response.Content.ReadAsStreamAsync();
                    var document = await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);

                    var quickFixes = ReadQuickFixes(asset.Id, document, !tag.IsPreRelease);
                    var inspections = ReadInspections(asset.Id, document, !tag.IsPreRelease, quickFixes);
                    return inspections.Concat(quickFixes);
                }
            }

            return Enumerable.Empty<FeatureItem>();
        }

        private IEnumerable<FeatureItem> ReadInspections(int assetId, XDocument doc, bool hasReleased, IEnumerable<FeatureItem> quickFixes) =>
            from node in doc.Descendants("member")
            let name = GetInspectionNameOrDefault(node)
            where !string.IsNullOrWhiteSpace(name)
            let inspectionName = name.Substring(name.LastIndexOf(".", StringComparison.Ordinal) + 1)
            let config = _inspectionDefaults.ContainsKey(inspectionName) ? _inspectionDefaults[inspectionName] : default
            select new XmlDocInspection(name, node, config, !hasReleased).Parse(assetId, quickFixes);

        private static string GetInspectionNameOrDefault(XElement memberNode)
        {
            var name = memberNode.Attribute("name")?.Value;
            if (name == null || !name.StartsWith("T:") || !name.EndsWith("Inspection") || name.EndsWith("IInspection"))
            {
                return default;
            }

            return name;
        }

        private IEnumerable<FeatureItem> ReadQuickFixes(int assetId, XDocument doc, bool hasReleased) =>
            from node in doc.Descendants("member")
            let name = GetQuickFixNameOrDefault(node)
            where !string.IsNullOrEmpty(name)
            && node.Descendants(XmlDocSchema.QuickFix.CanFix.ElementName).Any() // this excludes any quickfixes added to main (master) prior to v2.5.0 
            select new XmlDocQuickFix(_syntaxHighlighterService, name, node, !hasReleased).Parse(assetId);

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
