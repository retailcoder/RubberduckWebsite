using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rubberduck.API.Services.Abstract;
using Rubberduck.ContentServices;
using Rubberduck.ContentServices.XmlDoc.Abstract;
using Rubberduck.Model.Internal;

namespace Rubberduck.API.Services
{
    internal class XmlDocServices : IXmlDocServices
    {
        private readonly ILogger _logger;
        private readonly RubberduckDbContext _context;

        private readonly IGitHubDataServices _gitHub;
        private readonly IXmlDocParser _codeAnalysisXml;
        private readonly IXmlDocParser _parsingXml;
        private readonly IXmlDocMerge _xmlMerge;
        private readonly IContentServices _content;

        public XmlDocServices(RubberduckDbContext dbContext, ILogger<XmlDocServices> logger, IGitHubDataServices gitHub, IXmlDocMerge xmlMerge, ICodeAnalysisXmlDocParser codeAnalysisXml, IParsingXmlDocParser parsingXml, IContentServices content)
        {
            _context = dbContext;
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
            _logger.LogInformation($"Downloaded {tags.Count():N0} tags from GitHub repository.");
            tags = await SaveTags(tags);
            
            var main = tags.Where(tag => !tag.IsPreRelease).OrderByDescending(tag => tag.DateCreated).First();
            var mainTagId = main.Id;
            
            main = await _gitHub.GetTag(main.Name, mainTagId);
            _logger.LogInformation($"Downloaded {main.Assets.Count():N0} assets under tag {main.Name} (main).");

            var mainAssets = await SaveTagAssets(mainTagId, main.Assets);

            var next = tags.Where(tag => tag.IsPreRelease).OrderByDescending(tag => tag.DateCreated).First();            
            var nextTagId = next.Id;

            next = await _gitHub.GetTag(next.Name, nextTagId);
            _logger.LogInformation($"Downloaded {next.Assets.Count():N0} assets under tag {next.Name} (next).");

            var nextAssets = await SaveTagAssets(nextTagId, next.Assets);

            await _context.SaveChangesAsync(); // we need the materialized tag asset IDs for what follows...

            next = Tag.FromDTO(Tag.ToDTO(next), nextAssets);
            main = Tag.FromDTO(Tag.ToDTO(main), mainAssets);

            _logger.LogTrace($"Parsing xmldocs...");
            var mainCodeAnalysisItems = await _codeAnalysisXml.ParseAsync(main);
            var nextCodeAnalysisItems = await _codeAnalysisXml.ParseAsync(next);

            var mainParsingItems = await _parsingXml.ParseAsync(main);
            var nextParsingItems = await _parsingXml.ParseAsync(next);

            var mainItems = mainCodeAnalysisItems.Concat(mainParsingItems).ToList();
            var nextItems = nextCodeAnalysisItems.Concat(nextParsingItems).ToList();

            _logger.LogTrace($"Merging...");
            var merged = _xmlMerge.Merge(mainItems, nextItems);
            foreach (var item in merged)
            {
                try
                {
                    await _content.SaveAsync(FeatureItem.ToDTO(item));
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, $"An error has occurred while saving FeatureItem '{item.Name}'.");
                }
            }
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
