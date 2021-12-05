using System.Threading.Tasks;
using Rubberduck.Model.DTO;

namespace Rubberduck.API.Services.Abstract
{
    /// <summary>
    /// Encapsulates CRUD operations against content entities.
    /// </summary>
    public interface IContentServices
    {
        Task<Model.Entity.Feature> SaveAsync(Feature dto);
        Task<Model.Entity.FeatureItem> SaveAsync(FeatureItem dto);
        Task<Model.Entity.Example> SaveAsync(Example dto);
        Task<Model.Entity.ExampleModule> SaveAsync(ExampleModule dto);
        Task<Model.Entity.Tag> SaveAsync(Tag dto);
        Task<Model.Entity.TagAsset> SaveAsync(TagAsset dto);
    }
}
