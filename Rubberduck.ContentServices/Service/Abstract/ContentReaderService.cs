using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.Model.Entity;

namespace Rubberduck.ContentServices.Service.Abstract
{
    public abstract class ContentReaderService<TEntity, TDTO> : IContentReaderService<TEntity>
        where TEntity : IEntity
        where TDTO : Model.DTO.BaseDto
    {
        protected abstract TDTO GetDTO(TEntity entity);
        protected abstract TEntity GetEntity(TDTO dto);
        protected abstract IAsyncReadRepository<TDTO> Repository { get; }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => 
            await Repository.GetAllAsync().ContinueWith(t => t.Result.Select(GetEntity));

        public virtual async Task<TEntity> GetByEntityKeyAsync(object key) => 
            GetEntity(await Repository.GetByKeyAsync(key));

        public virtual async Task<TEntity> GetByIdAsync(int id) => 
            GetEntity(await Repository.GetByIdAsync(id));
    }
}
