using System.Threading.Tasks;
using Rubberduck.Model.Entity;

namespace Rubberduck.ContentServices.XmlDoc.Abstract
{
    public interface IXmlDocParser
    {
        Task<FeatureItem> ParseAsync();
    }

    public interface IInspectionXmlDocParser : IXmlDocParser { /* DI/IoC marker interface */ }
    public interface IQuickFixXmlDocParser  : IXmlDocParser { /* DI/IoC marker interface */ }
    public interface IAnnotationXmlDocParser : IXmlDocParser { /* DI/IoC marker interface */ }
}
