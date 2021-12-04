using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Rubberduck.Model.DTO;
using Rubberduck.ContentServices.Extensions;
using System.Reflection;

namespace Rubberduck.ContentServices.Repository.Abstract
{
    public abstract class AsyncDapperSqlRepositoryBase<T> : IAsyncReadRepository<T>, IAsyncWriteRepository<T>
        where T : BaseDto
    {
        protected AsyncDapperSqlRepositoryBase(IDbConnection connection, string fkProperty = null)
        {
            DbConnection = connection;
            FkProperty = fkProperty;

            SqlSelect = typeof(T).ToSqlSelect();
            SqlSelectFk = fkProperty is null ? null : typeof(T).ToSqlSelect(fkProperty);
            SqlInsert = typeof(T).ToSqlInsert();
            SqlUpdate = typeof(T).ToSqlUpdate(new[] { fkProperty });
            SqlDelete = typeof(T).ToSqlDelete();
        }

        protected string FkProperty { get; }
        protected string SqlSelectFk { get; }
        protected string SqlSelect { get; }
        protected string SqlInsert { get; }
        protected string SqlUpdate { get; }
        protected string SqlDelete { get; }
 
        protected IDbConnection DbConnection { get; }

        public IAsyncReadRepository<T> AsReader() => this;

        public virtual async Task CreateAsync(T dto)
        {
            await DbConnection.ExecuteAsync(SqlInsert, dto);
        }

        public virtual async Task DeleteAsync(T dto)
        {
            await DbConnection.ExecuteAsync(SqlDelete, dto);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(int? fkId = null)
        {
            if (fkId != null)
            {
                _ = FkProperty ?? throw new ArgumentException("Parameter was supplied but unexpected.", nameof(fkId));
            }
            else if (FkProperty != null)
            {
                _ = fkId ?? throw new ArgumentNullException(nameof(fkId), "Parameter was expected but not supplied.");
            }

            return await DbConnection.QueryAsync<T>(SqlSelectFk ?? SqlSelect, FkProperty is null ? null : new { id = fkId });
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            var sql = $"{SqlSelect} WHERE [Id] = @id;";
            return await DbConnection.QueryAsync<T>(sql, new { id }).ContinueWith(t => t.Result?.SingleOrDefault(), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public virtual async Task<T> GetByKeyAsync(object key)
        {
            var keyProperties = key.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite && p.CanRead)
                .ToDictionary(p => p.Name, p => p.GetValue(key));
            
            var keyParams = keyProperties.Select(kvp => $"{kvp.Key.Quote()} = @{kvp.Key}").ToArray();
            
            var sql = $"{SqlSelect} WHERE {string.Join(" AND ", keyParams)}";
            return await DbConnection.QueryAsync<T>(sql, key).ContinueWith(t => t.Result?.SingleOrDefault(), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public virtual async Task UpdateAsync(T dto)
        {
            await DbConnection.ExecuteAsync(SqlUpdate, dto);
        }
    }
}
