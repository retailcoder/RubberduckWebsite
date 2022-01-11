using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rubberduck.ContentServices.XmlDoc.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.API.Services.Abstract;

namespace Rubberduck.API.Services
{
    internal class XmlDocServices : IXmlDocServices
    {
        private readonly ILogger _logger;

        private readonly IGitHubDataServices _gitHub;
        private readonly IXmlDocParser _codeAnalysisXml;
        private readonly IXmlDocParser _parsingXml;
        private readonly IXmlDocMerge _xmlMerge;
        private readonly IContentService _content;

        public XmlDocServices(ILogger<XmlDocServices> logger, 
            IGitHubDataServices gitHub,
            IXmlDocMerge xmlMerge, 
            ICodeAnalysisXmlDocParser codeAnalysisXml, 
            IParsingXmlDocParser parsingXml, 
            IContentService content)
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
            await SynchronizeTags();

            _logger.LogInformation($"Loading tag assets...");
            var dbMain = await _content.GetMainTagAsync();
            _logger.LogInformation($"Loaded {dbMain.TagAssets.Count:N0} assets from database under tag {dbMain.Name} (main).");

            var dbNext = await _content.GetNextTagAsync();
            _logger.LogInformation($"Loaded {dbNext.TagAssets.Count:N0} assets from database under tag {dbNext.Name} (next).");

            _logger.LogInformation($"Parsing xmldocs for Rubberduck.CodeAnalysis asset...");
            var mainCodeAnalysisItems = await _codeAnalysisXml.ParseAsync(dbMain);
            var nextCodeAnalysisItems = await _codeAnalysisXml.ParseAsync(dbNext);

            _logger.LogInformation($"Parsing xmldocs for Rubberduck.Parsing asset...");
            var mainParsingItems = await _parsingXml.ParseAsync(dbMain);
            var nextParsingItems = await _parsingXml.ParseAsync(dbNext);

            _logger.LogInformation($"Concatenating parsed feature items...");
            var mainItems = mainCodeAnalysisItems.Concat(mainParsingItems).ToList();
            var nextItems = nextCodeAnalysisItems.Concat(nextParsingItems).ToList();

            _logger.LogInformation($"Merging...");
            var merged = _xmlMerge.Merge(mainItems, nextItems);

            _logger.LogInformation($"Saving changes...");
            await _content.SaveFeatureItemsAsync(merged);

            _logger.LogInformation($"Done.");
        }

        private async Task SynchronizeTags()
        {
            _logger.LogInformation($"Downloading all tags...");
            var githubTags = await _gitHub.GetAllTagsAsync(); // does not get the assets

            var sortedGithubTags = githubTags.OrderByDescending(tag => tag.DateCreated)
                .GroupBy(tag => tag.IsPreRelease)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.AsEnumerable());

            var mainTag = sortedGithubTags[false].First();
            var nextTag = sortedGithubTags[true].First();
            _logger.LogInformation($"Latest tags: {mainTag.Name} (main: {mainTag.InstallerDownloads:N0} downloads), {nextTag.Name} (next: {nextTag.InstallerDownloads:N0} downloads)");

            var main = await _gitHub.GetTagAsync(mainTag.Name);
            foreach (var asset in main.TagAssets)
            {
                mainTag.TagAssets.Add(asset);
            }
            _logger.LogInformation($"Downloaded {main.TagAssets.Count} assets under tag {main.Name}.");

            var next = await _gitHub.GetTagAsync(nextTag.Name);
            foreach (var asset in next.TagAssets)
            {
                nextTag.TagAssets.Add(asset);
            }
            _logger.LogInformation($"Downloaded {next.TagAssets.Count} assets under tag {next.Name}.");

            _logger.LogInformation($"Saving...");
            await _content.SaveTagsAsync(new[] { mainTag, nextTag });

            _logger.LogInformation($"Updating installer downloads for all other tags...");
            await _content.SaveTagsAsync(githubTags.Where(tag => tag.Name != main.Name && tag.Name != next.Name));
        }
    }
}
