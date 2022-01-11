using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Rubberduck.ContentServices.XmlDoc.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.ContentServices.Model;
using RubberduckServices.Abstract;
using System.Collections.Concurrent;

namespace Rubberduck.ContentServices.XmlDoc
{
    public class ParsingXmlDocParser : XmlDocParserBase, IParsingXmlDocParser
    {
        private readonly IContentService _content;
        private readonly ISyntaxHighlighterService _syntaxHighlighterService;
        
        public ParsingXmlDocParser(IContentService content, ISyntaxHighlighterService syntaxHighlighterService)
            : base("Rubberduck.Parsing.xml")
        {
            _content = content;
            _syntaxHighlighterService = syntaxHighlighterService;
        }

        protected override async Task<IEnumerable<FeatureItem>> ParseAsync(int assetId, XDocument document, bool isPreRelease)
        {
            var featureId = (await _content.GetFeatureAsync("Annotations"))?.Id
                ?? throw new InvalidOperationException("Could not retrieve a FeatureId for the 'Annotations' feature.");
            return await Task.FromResult(ReadAnnotations(assetId, featureId, document, !isPreRelease));
        }

        private IEnumerable<FeatureItem> ReadAnnotations(int assetId, int featureId, XDocument doc, bool hasReleased)
        {
            var nodes = from node in doc.Descendants("member").AsParallel()
                        let name = GetAnnotationNameOrDefault(node)
                        where !string.IsNullOrWhiteSpace(name)
                        select (name, node);

            var results = new ConcurrentBag<FeatureItem>();
            Parallel.ForEach(nodes, info =>
            {
                var xmldoc = new XmlDocAnnotation(_syntaxHighlighterService, info.name, info.node, !hasReleased);
                results.Add(xmldoc.Parse(assetId, featureId));
            });

            return results;

        }

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
