using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rubberduck.API.Services.Abstract;
using Rubberduck.ContentServices.XmlDoc.Abstract;
using Rubberduck.Model.Internal;

namespace Rubberduck.API.Services
{
    internal class XmlDocServices : IXmlDocServices
    {
        private readonly ILogger _logger;

        private readonly IGitHubDataServices _gitHub;
        private readonly IXmlDocParser _codeAnalysisXml;
        private readonly IXmlDocParser _parsingXml;
        private readonly IXmlDocMerge _xmlMerge;
        private readonly IContentServices _content;

        public XmlDocServices(ILogger<XmlDocServices> logger, IGitHubDataServices gitHub, IXmlDocMerge xmlMerge, ICodeAnalysisXmlDocParser codeAnalysisXml, IParsingXmlDocParser parsingXml, IContentServices content)
        {
            _logger = logger;

            _gitHub = gitHub;
            _xmlMerge = xmlMerge;
            _codeAnalysisXml = codeAnalysisXml;
            _parsingXml = parsingXml;
            _content = content;
        }

        public async Task SynchronizeAsync()
        {
            var tags = await _gitHub.GetAllTags();
            _logger.LogDebug($"Downloaded {tags.Count():N0} tags from GitHub repository.");
            tags = await SaveTags(tags);
            
            var main = tags.Where(tag => !tag.IsPreRelease).OrderByDescending(tag => tag.DateCreated).First();
            var mainTagId = main.Id;
            main = await _gitHub.GetTag(main.Name);
            _logger.LogDebug($"Downloaded {main.Assets.Count():N0} assets under tag {main.Name} (main).");
            await SaveTagAssets(mainTagId, main.Assets);

            var next = tags.Where(tag => tag.IsPreRelease).OrderByDescending(tag => tag.DateCreated).First();
            var nextTagId = next.Id;
            next = await _gitHub.GetTag(next.Name);
            _logger.LogDebug($"Downloaded {next.Assets.Count():N0} assets under tag {next.Name} (next).");
            await SaveTagAssets(nextTagId, next.Assets);

            _logger.LogInformation($"Parsing xmldocs...");
            var mainCodeAnalysisItems = await _codeAnalysisXml.ParseAsync(main);
            var nextCodeAnalysisItems = await _codeAnalysisXml.ParseAsync(next);

            var mainParsingItems = await _parsingXml.ParseAsync(main);
            var nextParsingItems = await _parsingXml.ParseAsync(next);

            var mainItems = (mainCodeAnalysisItems).Concat(mainParsingItems);
            var nextItems = (nextCodeAnalysisItems).Concat(nextParsingItems);

            _logger.LogInformation($"Merging...");
            var merged = _xmlMerge.Merge(mainItems, nextItems);
            foreach (var item in merged)
            {
                await _content.SaveAsync(FeatureItem.ToDTO(item));
            }
            _logger.LogInformation($"Done.");
        }

        private async Task<IEnumerable<Tag>> SaveTags(IEnumerable<Tag> tags)
        {
            var results = new List<Tag>();
            foreach (var tag in tags)
            {
                results.Add(await _content.SaveAsync(Tag.ToDTO(tag)));
            }
            return results;
        }

        private async Task<IEnumerable<TagAsset>> SaveTagAssets(int tagId, IEnumerable<TagAsset> tagAssets)
        {
            var results = new List<TagAsset>();
            foreach (var asset in tagAssets)
            {
                var dto = TagAsset.ToDTO(asset);
                dto.TagId = tagId;
                results.Add(await _content.SaveAsync(dto));
            }
            return results;
        }
    }
}
