using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Entity;

namespace Rubberduck.ContentServices.Reader
{
    public class FeatureItemContentReaderService : ContentReaderService<FeatureItem, Model.DTO.FeatureItem>
    {
        private readonly IReaderDbContext _context;

        public FeatureItemContentReaderService(IReaderDbContext context)
        {
            _context = context;
        }

        protected override IAsyncReadRepository<Model.DTO.FeatureItem> Repository => _context.FeatureItemsRepository;

        protected override Model.DTO.FeatureItem GetDTO(FeatureItem entity) => FeatureItem.ToDTO(entity);
        protected override FeatureItem GetEntity(Model.DTO.FeatureItem dto) => FeatureItem.FromDTO(dto);
        
        public override async Task<IEnumerable<FeatureItem>> GetAllAsync() =>
            await await Repository.GetAllAsync().ContinueWith(async t => await GetFeatureItemsAsync(t.Result));

        public override async Task<FeatureItem> GetByIdAsync(int id) =>
            await await Repository.GetByIdAsync(id).ContinueWith(async t => await GetFeatureItemAsync(t.Result));

        public override async Task<FeatureItem> GetByEntityKeyAsync(object key) =>
            await await Repository.GetByKeyAsync(key).ContinueWith(async t => await GetFeatureItemAsync(t.Result));

        private async Task<IEnumerable<FeatureItem>> GetFeatureItemsAsync(IEnumerable<Model.DTO.FeatureItem> featureItems)
        {
            var items = new List<FeatureItem>();
            foreach (var dto in featureItems)
            {
                var item = await GetFeatureItemAsync(dto);
                items.Add(item);
            }
            return items;
        }

        private async Task<FeatureItem> GetFeatureItemAsync(Model.DTO.FeatureItem dto)
        {
            if (dto is null)
            {
                return null;
            }
            if (dto.XmlDocSourceObject is not null)
            {
                var examples = await GetExamplesAsync(dto);
                return FeatureItem.FromDTO(dto, examples);
            }
            else
            {
                return FeatureItem.FromDTO(dto);
            }
        }

        private async Task<IEnumerable<Example>> GetExamplesAsync(Model.DTO.FeatureItem dto)
        {
            if (dto is null)
            {
                return Enumerable.Empty<Example>();
            }
            return await await _context.ExamplesRepository.GetAllAsync(dto.Id)
                .ContinueWith(async t =>
                {
                    var examples = new List<Example>();
                    foreach (var example in t.Result)
                    {
                        var modules = await GetExampleModulesAsync(example);
                        examples.Add(Example.FromDTO(example, modules));
                    }
                    return examples;
                });
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
