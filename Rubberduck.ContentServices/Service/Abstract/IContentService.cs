using System.Collections.Generic;
using System.Threading.Tasks;
using Rubberduck.ContentServices.Model;
using Rubberduck.Model;
using PublicModel = Rubberduck.Model.Entities;

namespace Rubberduck.ContentServices.Service.Abstract
{
    public interface IContentService
    {
        /// <summary>
        /// Gets all top-level features, including their sub-features and items.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<PublicModel.Feature>> GetFeaturesAsync();
        /// <summary>
        /// Gets the specified feature or sub-feature, including its items and/or sub-features.
        /// </summary>
        /// <param name="name">The name of the feature or sub-feature to retrieve</param>
        Task<PublicModel.Feature> GetFeatureAsync(string name);
        /// <summary>
        /// Gets the specified feature item, including any xmldoc examples.
        /// </summary>
        /// <param name="name">The name of the feature item to retrieve</param>
        /// <returns></returns>
        Task<PublicModel.FeatureItem> GetFeatureItemAsync(string name);
        /// <summary>
        /// Gets the latest release tag metadata from the <c>main</c> branch.
        /// </summary>
        Task<PublicModel.Tag> GetMainTagAsync();
        /// <summary>
        /// Gets the tag metadata for the latest pre-release tag from the <c>next</c> branch.
        /// </summary>
        Task<PublicModel.Tag> GetNextTagAsync();

        Task<PublicModel.Feature> SaveFeatureAsync(PublicModel.Feature model);
        Task<PublicModel.FeatureItem> SaveFeatureItemAsync(PublicModel.FeatureItem model);
        Task<IEnumerable<PublicModel.FeatureItem>> SaveFeatureItemsAsync(IEnumerable<FeatureItem> models);
        Task<PublicModel.Example> SaveExampleAsync(PublicModel.Example model);
        Task<PublicModel.ExampleModule> SaveExampleModuleAsync(PublicModel.ExampleModule model);
        Task<IEnumerable<PublicModel.Tag>> SaveTagsAsync(IEnumerable<PublicModel.Tag> models);

        Task<PublicModel.Feature> DeleteFeatureAsync(PublicModel.Feature model);
        Task<PublicModel.FeatureItem> DeleteFeatureItemAsync(PublicModel.FeatureItem model);
        Task<SearchResultsViewModel> SearchAsync(string search);
    }
}
