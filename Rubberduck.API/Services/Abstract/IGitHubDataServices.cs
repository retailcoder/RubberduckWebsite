using System.Collections.Generic;
using System.Threading.Tasks;
using Rubberduck.Model.Internal;
using Rubberduck.Model.ViewModel;

namespace Rubberduck.API.Services.Abstract
{
    /// <summary>
    /// A service that queries GitHub.
    /// </summary>
    public interface IGitHubDataServices
    {
        /// <summary>
        /// Gets the specified tag, or the latest release tag if not specified.
        /// </summary>
        /// <param name="name">The name of the tag to retrieve.</param>
        Task<Tag> GetTag(string name = null);
        /// <summary>
        /// Gets all tags, without their assets.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Tag>> GetAllTags();

        /// <summary>
        /// Gets the inspection types and severity overrides for each code inspection.
        /// </summary>
        Task<IEnumerable<InspectionDefaultConfig>> GetCodeAnalysisDefaultsConfig();
    }
}
