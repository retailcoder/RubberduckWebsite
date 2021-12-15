using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Rubberduck.ContentServices.XmlDoc.Abstract;
using Rubberduck.Model.Internal;

namespace Rubberduck.ContentServices.XmlDoc
{
    public class XmlDocMerge : IXmlDocMerge
    {
        private readonly ILogger _logger;

        public XmlDocMerge(ILogger<XmlDocMerge> logger)
        {
            _logger = logger;
        }

        public IEnumerable<FeatureItem> Merge(IEnumerable<FeatureItem> main, IEnumerable<FeatureItem> next)
        {
            var mainBranch = main.ToHashSet();
            var nextBranch = next.ToHashSet();
            _logger.LogInformation($"Merging {mainBranch.Count} feature items from [main] and {nextBranch.Count} feature items from [next].");

            var deletedItems = mainBranch.Where(e => !nextBranch.Contains(e))
                .Select(e => FeatureItem.ToDTO(e, isDiscontinued: true))
                .Select(FeatureItem.FromDTO)
                .ToHashSet();
            _logger.LogDebug($"{deletedItems.Count} feature items found in [main] but not in [next] will be marked as discontinued.");

            var newItems = nextBranch.Where(e => !mainBranch.Contains(e))
                .Select(e => FeatureItem.ToDTO(e, isNew:true))
                .Select(FeatureItem.FromDTO)
                .ToHashSet();
            _logger.LogDebug($"{newItems.Count} feature items found in [next] but not in [main] will be marked as new.");

            var otherItems = nextBranch.Intersect(mainBranch)
                .Select(e => FeatureItem.ToDTO(e, isNew:false))
                .Select(FeatureItem.FromDTO)
                .ToHashSet();
            _logger.LogDebug($"{otherItems.Count} feature items found in both [main] and [next] branches will use content from [next].");

            return otherItems.Concat(newItems).Concat(deletedItems).ToHashSet();
        }
    }
}
