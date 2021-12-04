using System;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Entity;

namespace Rubberduck.ContentServices.Writer
{
    public class ExampleModuleContentWriterService : ContentWriterService<ExampleModule, Model.DTO.ExampleModule>
    {
        private readonly IWriterDbContext _context;

        public ExampleModuleContentWriterService(IWriterDbContext context)
        {
            _context = context;
        }

        protected override IAsyncWriteRepository<Model.DTO.ExampleModule> Repository => _context.ExampleModulesRepository;
        protected override ExampleModule GetEntity(Model.DTO.ExampleModule dto) => ExampleModule.FromDTO(dto);
        protected override Model.DTO.ExampleModule GetDTO(ExampleModule entity) => ExampleModule.ToDTO(entity);
    }
}
