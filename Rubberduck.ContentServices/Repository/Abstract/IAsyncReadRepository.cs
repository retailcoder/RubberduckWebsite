using System.Collections.Generic;
using System.Threading.Tasks;
using Rubberduck.Model.DTO;

namespace Rubberduck.ContentServices.Repository.Abstract
{
    public interface IAsyncReadRepository<T>
        where T : BaseDto
    {
        Task<IEnumerable<T>> GetAllAsync(int? fkId = null);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByKeyAsync(object key);
    }
}
