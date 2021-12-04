using System.Threading.Tasks;

namespace Rubberduck.ContentServices.Service.Abstract
{
    public interface IContentWriterService<TEntity>
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
