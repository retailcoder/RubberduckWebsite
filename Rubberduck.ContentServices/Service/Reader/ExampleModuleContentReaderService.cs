using System.Threading.Tasks;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Entity;

namespace Rubberduck.ContentServices.Reader
{
    public class ExampleModuleContentReaderService : ContentReaderService<ExampleModule, Model.DTO.ExampleModule>
    {
        private readonly IReaderDbContext _context;

        public ExampleModuleContentReaderService(IReaderDbContext context)
        {
            _context = context;
        }

        protected override IAsyncReadRepository<Model.DTO.ExampleModule> Repository => _context.ExampleModulesRepository;
        protected override Model.DTO.ExampleModule GetDTO(ExampleModule entity) => ExampleModule.ToDTO(entity);
        protected override ExampleModule GetEntity(Model.DTO.ExampleModule dto) => ExampleModule.FromDTO(dto);

        public override async Task<ExampleModule> GetByIdAsync(int id) =>
            await Repository.GetByIdAsync(id).ContinueWith(t => ExampleModule.FromDTO(t.Result));

        public override async Task<ExampleModule> GetByEntityKeyAsync(object key) =>
            await Repository.GetByKeyAsync(key).ContinueWith(t => ExampleModule.FromDTO(t.Result));
    }
}
