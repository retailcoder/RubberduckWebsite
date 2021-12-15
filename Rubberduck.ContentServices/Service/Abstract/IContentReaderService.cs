﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rubberduck.ContentServices.Service.Abstract
{
    public interface IContentReaderService<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByEntityKeyAsync(TEntity key);
        Task<TEntity> GetByIdAsync(int id);
    }
}
