using System;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Entity;

namespace Rubberduck.ContentServices.Writer
{
    public class ExampleContentWriterService : ContentWriterService<Example, Model.DTO.Example>
    {
        private readonly IWriterDbContext _context;

        public ExampleContentWriterService(IWriterDbContext context)
        {
            _context = context;
        }

        protected override IAsyncWriteRepository<Model.DTO.Example> Repository => _context.ExamplesRepository;
        protected override Example GetEntity(Model.DTO.Example dto) => Example.FromDTO(dto);
        protected override Model.DTO.Example GetDTO(Example entity) => Example.ToDTO(entity);
    }
}
