using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Rubberduck.ContentServices.XmlDoc.Abstract;
using Rubberduck.Model.Internal;
using RubberduckServices.Abstract;

namespace Rubberduck.ContentServices.XmlDoc
{
    public class ParsingXmlDocParser : XmlDocParserBase, IParsingXmlDocParser
    {
        private readonly ISyntaxHighlighterService _syntaxHighlighterService;
        
        public ParsingXmlDocParser(ISyntaxHighlighterService syntaxHighlighterService)
            : base("Rubberduck.Parsing")
        {
            _syntaxHighlighterService = syntaxHighlighterService;
        }

        protected override async Task<IEnumerable<FeatureItem>> ParseAsync(int assetId, XDocument document, bool isPreRelease) =>
            await Task.FromResult(ReadAnnotations(assetId, document, !isPreRelease));

        private IEnumerable<FeatureItem> ReadAnnotations(int assetId, XDocument doc, bool hasReleased) =>
            from node in doc.Descendants("member")
            let name = GetAnnotationNameOrDefault(node)
            where !string.IsNullOrWhiteSpace(name)
            select new XmlDocAnnotation(_syntaxHighlighterService, name, node, !hasReleased).Parse(assetId);

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
