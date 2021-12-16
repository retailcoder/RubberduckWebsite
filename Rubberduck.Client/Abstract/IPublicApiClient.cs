using System.Collections.Generic;
using System.Threading.Tasks;
using Rubberduck.Model.Abstract;
using Rubberduck.Model.DTO;

namespace Rubberduck.Client.Abstract
{
    /// <summary>
    /// An API client that provides website content through anonymous requests.
    /// </summary>
    public interface IPublicApiClient
    {
        /// <summary>
        /// Gets all features, sub-features, and their respective feature items.
        /// </summary>
        /// <remarks>
        /// A separate request is required to retrieve a particular feature item's details.
        /// </remarks>
        Task<IEnumerable<Feature>> GetFeaturesAsync();
        /// <summary>
        /// Gets the specified feature item, including its examples and their respective modules.
        /// </summary>
        /// <param name="id">Uniquely identifies the feature item to get.</param>
        Task<FeatureItem> GetFeatureItemAsync(int id);
        /// <summary>
        /// Gets metadata for the latest release and prerelease tags.
        /// </summary>
        Task<IEnumerable<Tag>> GetLatestTagsAsync();
        /// <summary>
        /// Indents the supplied code as per specified indenter settings.
        /// </summary>
        Task<string[]> GetIndentedCodeAsync(IIndenterSettings settings);
    }
}
