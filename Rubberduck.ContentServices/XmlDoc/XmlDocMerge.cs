using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Rubberduck.ContentServices.XmlDoc.Abstract;
using Rubberduck.ContentServices.Model;

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

            var deletedItems = new HashSet<FeatureItem>(
                from item in mainBranch 
                where !nextBranch.Contains(item)
                select new FeatureItem(item.ToPublicModel()) { IsDiscontinued = true });

            _logger.LogDebug($"{deletedItems.Count} feature items found in [main] but not in [next] will be marked as discontinued.");

            var newItems = new HashSet<FeatureItem>(
                from item in nextBranch
                where !mainBranch.Contains(item)
                select new FeatureItem(item.ToPublicModel()) { IsNew = true });

            _logger.LogDebug($"{newItems.Count} feature items found in [next] but not in [main] will be marked as new.");

            var otherItems = new HashSet<FeatureItem>(
                from item in nextBranch.Intersect(mainBranch)
                select new FeatureItem(item.ToPublicModel()) { IsNew = false });

            _logger.LogDebug($"{otherItems.Count} feature items found in both [main] and [next] branches will use content from [next].");

            var merged = otherItems.Concat(newItems).Concat(deletedItems).ToHashSet();
            return merged;
        }
    }
}
