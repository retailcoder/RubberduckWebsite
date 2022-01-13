using System.Collections.Generic;
using System.Threading.Tasks;
using Rubberduck.Model;
using Rubberduck.Model.Entities;

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
        /// <param name="id">The <c>Id</c> to optionally set the DTO with.</param>
        Task<Tag> GetTagAsync(string name = null, int? id = null);
        /// <summary>
        /// Gets all tags, without their assets.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Tag>> GetAllTagsAsync();

        /// <summary>
        /// Gets the inspection types and severity overrides for each code inspection.
        /// </summary>
        Task<IEnumerable<InspectionDefaultConfig>> GetCodeAnalysisDefaultsConfig();
    }
}
