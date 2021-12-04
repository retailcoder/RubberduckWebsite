using System.Data;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.Model.DTO;

namespace Rubberduck.ContentServices.Repository
{
    internal class ExampleModulesRepository : AsyncDapperSqlRepositoryBase<ExampleModule>
    {
        public ExampleModulesRepository(IDbConnection connection)
            : base(connection, nameof(ExampleModule.ExampleId))
        {

        }
    }
}
