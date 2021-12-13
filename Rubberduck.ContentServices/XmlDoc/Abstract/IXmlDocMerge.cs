using System.Collections.Generic;
using Rubberduck.Model.Internal;

namespace Rubberduck.ContentServices.XmlDoc.Abstract
{
    public interface IXmlDocMerge
    {
        IEnumerable<FeatureItem> Merge(IEnumerable<FeatureItem> main, IEnumerable<FeatureItem> next);
    }
}
