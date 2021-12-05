using System.Threading.Tasks;

namespace Rubberduck.API.Services.Abstract
{
    /// <summary>
    /// A service that orchestrates the synchronization of xmldoc content.
    /// </summary>
    public interface IXmlDocServices
    {
        /// <summary>
        /// Gets the xmldoc assets from the latest release and prerelease tags, and processes/merges them into the database.
        /// </summary>
        Task SynchronizeAsync();
    }
}
