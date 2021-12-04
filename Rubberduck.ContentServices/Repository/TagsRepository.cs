using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.Model.DTO;

namespace Rubberduck.ContentServices.Repository
{
    internal class TagsRepository : AsyncDapperSqlRepositoryBase<Tag>
    {
        public TagsRepository(IDbConnection connection)
            : base(connection)
        {

        }

        public async Task<IEnumerable<Tag>> GetLatestTagsAsync()
        {
            return await DbConnection.QueryAsync<Tag>("SELECT * FROM [dbo].[vLatestTags]");
        }
    }
}
