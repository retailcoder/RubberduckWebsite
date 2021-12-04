using System.Data;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.Model.DTO;

namespace Rubberduck.ContentServices.Repository
{
    internal class FeatureItemsRepository : AsyncDapperSqlRepositoryBase<FeatureItem>
    {
        public FeatureItemsRepository(IDbConnection connection)
            : base(connection, nameof(FeatureItem.FeatureId))
        {

        }
    }
}
