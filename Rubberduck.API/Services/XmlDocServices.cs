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
            var githubTags = await _gitHub.GetAllTagsAsync();
            _logger.LogInformation($"Downloaded {githubTags.Count():N0} tags from GitHub repository.");
            await _content.SaveTagsAsync(githubTags);

            var dbMain = await _content.GetMainTagAsync();
            var githubMain = await _gitHub.GetTagAsync(dbMain.Name);
            githubMain.Id = dbMain.Id;
            _logger.LogInformation($"Downloaded {githubMain.TagAssets.Count:N0} assets under tag {githubMain.Name} (main).");

            var dbNext = await _content.GetNextTagAsync();
            var githubNext = await _gitHub.GetTagAsync(dbNext.Name);
            githubNext.Id = dbNext.Id;
            _logger.LogInformation($"Downloaded {dbNext.TagAssets.Count:N0} assets under tag {dbNext.Name} (next).");

            _logger.LogTrace($"Parsing xmldocs...");
            var mainCodeAnalysisItems = await _codeAnalysisXml.ParseAsync(githubMain);
            var nextCodeAnalysisItems = await _codeAnalysisXml.ParseAsync(githubNext);

            var mainParsingItems = await _parsingXml.ParseAsync(githubMain);
            var nextParsingItems = await _parsingXml.ParseAsync(githubNext);

            var mainItems = mainCodeAnalysisItems.Concat(mainParsingItems).ToList();
            var nextItems = nextCodeAnalysisItems.Concat(nextParsingItems).ToList();

            _logger.LogTrace($"Merging...");
            var merged = _xmlMerge.Merge(mainItems, nextItems);
            await _content.SaveFeatureItemsAsync(merged);
        }
    }
}
