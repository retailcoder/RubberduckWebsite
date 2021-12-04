using System.Data;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.Model.DTO;

namespace Rubberduck.ContentServices.Repository
{
    internal class TagAssetsRepository : AsyncDapperSqlRepositoryBase<TagAsset>
    {
        public TagAssetsRepository(IDbConnection connection)
            : base(connection)
        {

        }
    }
}
