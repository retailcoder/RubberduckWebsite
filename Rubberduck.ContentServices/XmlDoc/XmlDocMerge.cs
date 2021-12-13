using System.Collections.Generic;
using System.Linq;
using Rubberduck.ContentServices.XmlDoc.Abstract;
using Rubberduck.Model.Internal;

namespace Rubberduck.ContentServices.XmlDoc
{
    public class XmlDocMerge : IXmlDocMerge
    {
        public IEnumerable<FeatureItem> Merge(IEnumerable<FeatureItem> main, IEnumerable<FeatureItem> next)
        {
            var mainBranch = main.ToHashSet();
            var nextBranch = next.ToHashSet();

            var deletedItems = main.Where(e => !nextBranch.Contains(e))
                .Select(e => FeatureItem.ToDTO(e, isDiscontinued:true))
                .Select(FeatureItem.FromDTO);
            var newItems = next.Where(e => !mainBranch.Contains(e))
                .Select(e => FeatureItem.ToDTO(e, isNew:true))
                .Select(FeatureItem.FromDTO);
            var otherItems = nextBranch.Intersect(mainBranch);

            return otherItems.Concat(newItems).Concat(deletedItems).ToList();
        }
    }
}
