using System.Threading.Tasks;
using Rubberduck.Model.Abstract;

namespace RubberduckServices.Abstract
{
    public interface IIndenterService
    {
        Task<string[]> IndentAsync(IIndenterSettings settings);
    }
}
