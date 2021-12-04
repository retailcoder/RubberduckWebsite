using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Entity;

namespace Rubberduck.ContentServices.Reader
{
    public class ExampleContentReaderService : ContentReaderService<Example, Model.DTO.Example>
    {
        private readonly IReaderDbContext _context;

        public ExampleContentReaderService(IReaderDbContext context)
        {
            _context = context;
        }

        protected override IAsyncReadRepository<Model.DTO.Example> Repository => _context.ExamplesRepository;
        protected override Model.DTO.Example GetDTO(Example entity) => Example.ToDTO(entity);
        protected override Example GetEntity(Model.DTO.Example dto) => Example.FromDTO(dto);

        public override async Task<Example> GetByIdAsync(int id) =>
            await await Repository.GetByIdAsync(id).ContinueWith(async t => await GetExampleAsync(t.Result));

        public override async Task<Example> GetByEntityKeyAsync(object key) =>
            await await Repository.GetByKeyAsync(key).ContinueWith(async t => await GetExampleAsync(t.Result));

        private async Task<Example> GetExampleAsync(Model.DTO.Example dto)
        {
            if (dto is null)
            {
                return null;
            }
            var modules = await GetExampleModulesAsync(dto);
            return Example.FromDTO(dto, modules);
        }

        private async Task<IEnumerable<ExampleModule>> GetExampleModulesAsync(Model.DTO.Example dto)
        {
            if (dto is null)
            {
                return Enumerable.Empty<ExampleModule>();
            }
            return await _context.ExampleModulesRepository.GetAllAsync(dto.Id)
                .ContinueWith(t => t.Result.Select(ExampleModule.FromDTO));
        }
    }
}
