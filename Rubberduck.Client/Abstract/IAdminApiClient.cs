using System.Threading.Tasks;
using Rubberduck.Model.Entities;

namespace Rubberduck.Client.Abstract
{
    /// <summary>
    /// An API client that requires authentication and allows updating website content.
    /// </summary>
    public interface IAdminApiClient
    {
        /// <summary>
        /// Gets the latest release and pre-release tags, downloads xmldoc assets, and processes them.
        /// </summary>
        Task<bool> UpdateTagMetadataAsync();
        /// <summary>
        /// Creates a new feature or sub-feature, or updates an existing one.
        /// </summary>
        Task<Feature> SaveFeatureAsync(Feature dto);
        /// <summary>
        /// Creates a new feature item, or updates an existing one.
        /// </summary>
        Task<FeatureItem> SaveFeatureItemAsync(FeatureItem dto);
    }
}
