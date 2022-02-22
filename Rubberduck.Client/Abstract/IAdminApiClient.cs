using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Rubberduck.Model.Entities;

namespace Rubberduck.Client.Abstract
{
    /// <summary>
    /// An API client that requires authentication and allows updating website content.
    /// </summary>
    public interface IAdminApiClient : IPublicApiClient
    {
        /// <summary>
        /// Gets the latest release and pre-release tags, downloads xmldoc assets, and processes them.
        /// </summary>
        /// <exception cref="TaskCanceledException" />
        Task<bool> UpdateTagMetadataAsync();
        /// <summary>
        /// Creates a new feature or sub-feature, or updates an existing one.
        /// </summary>
        Task<Feature> SaveFeatureAsync(Feature dto);
        /// <summary>
        /// Creates a new feature item, or updates an existing one.
        /// </summary>
        Task<FeatureItem> SaveFeatureItemAsync(FeatureItem dto);

        /// <summary>
        /// Deletes a non-protected feature and all data associated to it.
        /// </summary>
        Task<Feature> DeleteFeatureAsync(Feature dto);
        /// <summary>
        /// Deletes a feature item and all data associated to it.
        /// </summary>
        Task<FeatureItem> DeleteFeatureItemAsync(FeatureItem dto);
        /// <summary>
        /// Gets an indicator that is <c>true</c> if a synchronisation is in progress at the time of the request.
        /// </summary>
        Task<bool> IsUpdatingAsync();
    }
}
