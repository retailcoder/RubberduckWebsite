using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Entity;

namespace Rubberduck.ContentServices.Reader
{
    public class FeatureContentReaderService : ContentReaderService<Feature, Model.DTO.Feature>
    {
        private readonly IReaderDbContext _context;

        public FeatureContentReaderService(IReaderDbContext context)
        {
            _context = context;
        }

        protected override IAsyncReadRepository<Model.DTO.Feature> Repository => _context.FeaturesRepository;

        protected override Model.DTO.Feature GetDTO(Feature entity) => Feature.ToDTO(entity);
        protected override Feature GetEntity(Model.DTO.Feature dto) => Feature.FromDTO(dto);

        public override async Task<IEnumerable<Feature>> GetAllAsync() => 
            await await Repository.GetAllAsync().ContinueWith(async t => await GetFeaturesAsync(t.Result));

        public override async Task<Feature> GetByIdAsync(int id) =>
            await await Repository.GetByIdAsync(id).ContinueWith(async t => await GetFeatureAsync(t.Result));

        public override async Task<Feature> GetByEntityKeyAsync(object key) =>
            await await Repository.GetByKeyAsync(key).ContinueWith(async t => await GetFeatureAsync(t.Result));

        private async Task<IEnumerable<Feature>> GetFeaturesAsync(IEnumerable<Model.DTO.Feature> features)
        {
            var results = new List<Feature>();
            foreach (var feature in features)
            {
                var model = await GetFeatureAsync(feature);
                results.Add(model);
            }
            return results;
        }

        private async Task<Feature> GetFeatureAsync(Model.DTO.Feature dto)
        {
            if (dto is null)
            {
                return null;
            }

            if (dto.XmlDocSource is null)
            {
                var subFeatures = await GetSubFeaturesAsync(dto);
                return Feature.FromDTO(dto, subFeatures);
            }
            else
            {
                var items = await GetFeatureItemsAsync(dto);
                return Feature.FromDTO(dto, items);
            }
        }

        private async Task<IEnumerable<Feature>> GetSubFeaturesAsync(Model.DTO.Feature dto)
        {
            if (dto is null)
            {
                return Enumerable.Empty<Feature>();
            }
            return await await Repository.GetAllAsync(dto.Id)
                .ContinueWith(async t =>
                {
                    var features = new List<Feature>();
                    foreach (var feature in t.Result)
                    {
                        if (feature.XmlDocSource is null)
                        {
                            var subFeatures = await GetSubFeaturesAsync(feature); // recursive
                            features.Add(Feature.FromDTO(feature, subFeatures));
                        }
                        else
                        {
                            var items = await GetFeatureItemsAsync(feature);
                            features.Add(Feature.FromDTO(feature, items));
                        }
                    }
                    return features;
                });
        }

        private async Task<IEnumerable<FeatureItem>> GetFeatureItemsAsync(Model.DTO.Feature dto)
        {
            if (dto is null)
            {
                return Enumerable.Empty<FeatureItem>();
            }
            return await _context.FeatureItemsRepository.GetAllAsync(dto.Id).ContinueWith(t => t.Result.Select(FeatureItem.FromDTO));
        }
    }
}
