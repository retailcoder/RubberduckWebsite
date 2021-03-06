using System.Collections.Generic;
using System.Threading.Tasks;
using Rubberduck.ContentServices.Model;

namespace Rubberduck.ContentServices.XmlDoc.Abstract
{
    public interface IXmlDocParser
    {
        string AssetName { get; }
        Task<IEnumerable<FeatureItem>> ParseAsync(Rubberduck.Model.Entities.Tag tag);
    }

    /// <summary>
    /// Downloads and processes Rubberduck.CodeAnalysis xmldoc asset for a tag.
    /// </summary>
    public interface ICodeAnalysisXmlDocParser : IXmlDocParser { /* DI/IoC marker interface */ }

    /// <summary>
    /// Downloads and processes Rubberduck.Parsing xmldoc asset for a tag.
    /// </summary>
    public interface IParsingXmlDocParser : IXmlDocParser { /* DI/IoC marker interface */ }
}
