using System.Data;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.Model.DTO;

namespace Rubberduck.ContentServices.Repository
{
    internal class ExamplesRepository : AsyncDapperSqlRepositoryBase<Example>
    {
        public ExamplesRepository(IDbConnection connection)
            : base(connection, nameof(Example.FeatureItemId))
        {

        }
    }
}
