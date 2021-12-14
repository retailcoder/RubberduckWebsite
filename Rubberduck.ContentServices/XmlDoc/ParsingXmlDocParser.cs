using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Rubberduck.ContentServices.XmlDoc.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Internal;
using RubberduckServices.Abstract;

namespace Rubberduck.ContentServices.XmlDoc
{
    public class ParsingXmlDocParser : XmlDocParserBase, IParsingXmlDocParser
    {
        private readonly IContentReaderService<Feature> _features;
        private readonly ISyntaxHighlighterService _syntaxHighlighterService;
        
        public ParsingXmlDocParser(IContentReaderService<Feature> features, ISyntaxHighlighterService syntaxHighlighterService)
            : base("Rubberduck.Parsing")
        {
            _features = features;
            _syntaxHighlighterService = syntaxHighlighterService;
        }

        protected override async Task<IEnumerable<FeatureItem>> ParseAsync(int assetId, XDocument document, bool isPreRelease)
        {
            var key = Feature.FromDTO(new Model.DTO.Feature { Name = "Annotations" });
            var featureId = (await _features.GetByEntityKeyAsync(key)).Id;
            return await Task.FromResult(ReadAnnotations(assetId, featureId, document, !isPreRelease));
        }

        private IEnumerable<FeatureItem> ReadAnnotations(int assetId, int featureId, XDocument doc, bool hasReleased) =>
            from node in doc.Descendants("member")
            let name = GetAnnotationNameOrDefault(node)
            where !string.IsNullOrWhiteSpace(name)
            select new XmlDocAnnotation(_syntaxHighlighterService, name, node, !hasReleased).Parse(assetId, featureId);

        private static string GetAnnotationNameOrDefault(XElement memberNode)
        {
            var name = memberNode.Attribute("name")?.Value;
            if (name == null || !name.StartsWith("T:") || !name.EndsWith("Annotation"))
            {
                return default;
            }

            return name;
        }
    }
}
