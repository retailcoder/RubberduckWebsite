using System.Data;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.Model.DTO;

namespace Rubberduck.ContentServices.Repository
{
    internal class FeaturesRepository : AsyncDapperSqlRepositoryBase<Feature>
    {
        public FeaturesRepository(IDbConnection connection) 
            : base(connection)
        {

        }
    }
}
