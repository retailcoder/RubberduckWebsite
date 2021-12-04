using System.Threading.Tasks;
using Rubberduck.Model.DTO;

namespace Rubberduck.ContentServices.Repository.Abstract
{
    public interface IAsyncWriteRepository<T>
        where T : BaseDto
    {
        Task CreateAsync(T dto);
        Task UpdateAsync(T dto);
        Task DeleteAsync(T dto);

        IAsyncReadRepository<T> AsReader();
    }
}
