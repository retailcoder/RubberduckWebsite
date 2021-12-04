using System.Threading.Tasks;

namespace RubberduckServices.Abstract
{
    /// <summary>
    /// Represents a service that can parse and format a VBA code string.
    /// </summary>
    public interface ISyntaxHighlighterService
    {
        /// <summary>
        /// Formats the specified code.
        /// </summary>
        /// <param name="code">A fragment of VBA code that can be parsed by Rubberduck.</param>
        /// <returns>The provided code, syntax-formatted.</returns>
        Task<string> FormatAsync(string code);
    }
}
