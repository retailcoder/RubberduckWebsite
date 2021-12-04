using System;
using System.Threading.Tasks;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.Model.Entity;

namespace Rubberduck.ContentServices.Service.Abstract
{
    public abstract class ContentWriterService<TEntity, TDTO> : IContentWriterService<TEntity>
        where TEntity : IEntity
        where TDTO : Model.DTO.BaseDto
    {
        protected abstract TDTO GetDTO(TEntity entity);
        protected abstract TEntity GetEntity(TDTO dto);
        protected abstract IAsyncWriteRepository<TDTO> Repository { get; }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var dto = GetDTO(entity) ?? throw new ArgumentNullException(nameof(entity));
            await Repository.CreateAsync(dto);
            var result = await Repository.AsReader().GetByKeyAsync(entity.Key());
            return GetEntity(result);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            var dto = GetDTO(entity) ?? throw new ArgumentNullException(nameof(entity));
            await Repository.DeleteAsync(dto);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var dto = GetDTO(entity) ?? throw new ArgumentNullException(nameof(entity));
            await Repository.UpdateAsync(dto);
            var result = await Repository.AsReader().GetByIdAsync(entity.Id);
            return GetEntity(result);
        }
    }
}
