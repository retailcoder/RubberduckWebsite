using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Internal;

namespace Rubberduck.ContentServices.Reader
{
    public class ExampleModuleContentReaderService : IContentReaderService<ExampleModule>
    {
        private readonly RubberduckDbContext _context;

        public ExampleModuleContentReaderService(RubberduckDbContext context)
        {
            _context = context;
        }

        private IQueryable<Model.DTO.ExampleModuleEntity> Repository =>
            _context.ExampleModules.AsNoTracking();

        public async Task<ExampleModule> GetByIdAsync(int id) =>
            await Task.FromResult(ExampleModule.FromDTO(Repository.Single(e => e.Id == id)));

        public async Task<ExampleModule> GetByEntityKeyAsync(ExampleModule key) =>
            await Task.FromResult(ExampleModule.FromDTO(Repository.SingleOrDefault(e => e.ExampleId == key.ExampleId && e.ModuleName == key.ModuleName)));

        public async Task<IEnumerable<ExampleModule>> GetAllAsync() =>
            await Task.FromResult(Repository.Select(ExampleModule.FromDTO).ToList());
    }
}
