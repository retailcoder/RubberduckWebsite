using System.Threading.Tasks;
using Rubberduck.Model.DTO;

namespace Rubberduck.API.Services.Abstract
{
    /// <summary>
    /// Encapsulates CRUD operations against content entities.
    /// </summary>
    public interface IContentServices
    {
        /// <summary>
        /// Inserts or updates the specified data object.
        /// </summary>
        Task<Model.Entity.Feature> SaveAsync(Feature dto);
        /// <summary>
        /// Inserts or updates the specified data object.
        /// </summary>
        Task<Model.Entity.FeatureItem> SaveAsync(FeatureItem dto);
        /// <summary>
        /// Inserts or updates the specified data object.
        /// </summary>
        Task<Model.Entity.Example> SaveAsync(Example dto);
        /// <summary>
        /// Inserts or updates the specified data object.
        /// </summary>
        Task<Model.Entity.ExampleModule> SaveAsync(ExampleModule dto);
        /// <summary>
        /// Inserts or updates the specified data object.
        /// </summary>
        Task<Model.Entity.Tag> SaveAsync(Tag dto);
        /// <summary>
        /// Inserts or updates the specified data object.
        /// </summary>
        Task<Model.Entity.TagAsset> SaveAsync(TagAsset dto);
    }
}
