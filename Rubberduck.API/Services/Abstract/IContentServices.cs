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
        Task<Model.Internal.Feature> SaveAsync(Feature dto);
        /// <summary>
        /// Inserts or updates the specified data object.
        /// </summary>
        Task<Model.Internal.FeatureItem> SaveAsync(FeatureItem dto);
        /// <summary>
        /// Inserts or updates the specified data object.
        /// </summary>
        Task<Model.Internal.Example> SaveAsync(Example dto);
        /// <summary>
        /// Inserts or updates the specified data object.
        /// </summary>
        Task<Model.Internal.ExampleModule> SaveAsync(ExampleModule dto);
        /// <summary>
        /// Inserts or updates the specified data object.
        /// </summary>
        Task<Model.Internal.Tag> SaveAsync(Tag dto);
        /// <summary>
        /// Inserts or updates the specified data object.
        /// </summary>
        Task<Model.Internal.TagAsset> SaveAsync(TagAsset dto);
    }
}
