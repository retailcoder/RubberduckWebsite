using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rubberduck.API.Services.Abstract;
using Rubberduck.Model.Internal;

namespace Rubberduck.API.Services
{
    internal class XmlDocServices : IXmlDocServices
    {
        private IGitHubDataServices _gitHub;
        //private IXmlDocParser _parser;
        private IContentServices _content;

        public XmlDocServices(IGitHubDataServices gitHub, /*IXmlDocParser parser,*/ IContentServices content)
        {
            _gitHub = gitHub;
            //_parser = parser;
            _content = content;
        }

        public async Task SynchronizeAsync()
        {
            var tags = await await _gitHub.GetAllTags()
                .ContinueWith(async t => 
                {
                    var tags = await t;
                    return await SaveTags(tags);
                });

            var getLatest = Task.Run(async () =>
            {
                var latest = tags.Where(tag => !tag.IsPreRelease).OrderByDescending(tag => tag.DateCreated).First();
                return await _gitHub.GetTag(latest.Name);
            })
            .ContinueWith(async t =>
            {
                var tag = await t;
                await SaveTagAssets(tag.Assets);
                return tag;
            });

            var getNext = Task.Run(async () =>
            {
                var next = tags.Where(tag => tag.IsPreRelease).OrderByDescending(tag => tag.DateCreated).First();
                return await _gitHub.GetTag(next.Name);
            })
            .ContinueWith(async t =>
            {
                var tag = await t;
                await SaveTagAssets(tag.Assets);
                return tag;
            });

            await Task.WhenAll(getLatest, getNext);
            var latest = await getLatest;
            var next = await getNext;

            // TODO download and parse xmldoc assets into FeatureItem entities.
        }

        private async Task<IEnumerable<Tag>> SaveTags(IEnumerable<Tag> tags)
        {
            return await Task.WhenAll(tags.Select(tag => _content.SaveAsync(Tag.ToDTO(tag))).ToArray());
        }

        private async Task<IEnumerable<TagAsset>> SaveTagAssets(IEnumerable<TagAsset> tagAssets)
        {
            return await Task.WhenAll(tagAssets.Select(asset => _content.SaveAsync(TagAsset.ToDTO(asset))).ToArray());
        }
    }
}
