using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Internal;
using Microsoft.EntityFrameworkCore;

namespace Rubberduck.ContentServices.Reader
{
    public class FeatureContentReaderService : IContentReaderService<Feature>
    {
        private readonly RubberduckDbContext _context;

        public FeatureContentReaderService(RubberduckDbContext context)
        {
            _context = context;
        }

        private IQueryable<Model.DTO.FeatureEntity> Repository =>
            _context.Features.AsNoTracking()
                .Include(e => e.FeatureItems)
                .Include(e => e.SubFeatures);

        public async Task<Feature> GetByIdAsync(int id)
        {
            var feature = Repository.Single(e => e.Id == id);
            var items = feature.FeatureItems.Select(FeatureItem.FromDTO).ToArray();
            var subFeatures = Traverse(feature).ToArray();
            return await Task.FromResult(Feature.FromDTO(feature, subFeatures, items));
        }

        private IEnumerable<Feature> Traverse(Model.DTO.FeatureEntity parent)
        {
            if (parent?.SubFeatures is null)
            {
                yield break;
            }
            foreach (var feature in parent.SubFeatures)
            {
                var items = feature.FeatureItems.Select(FeatureItem.FromDTO).ToArray();
                var subFeatures = Traverse(feature).ToArray();
                yield return Feature.FromDTO(feature, subFeatures, items);
            }
        }

        public async Task<Feature> GetByEntityKeyAsync(Feature key)
        {
            var feature = Repository.SingleOrDefault(e => e.Name == key.Name);
            if (feature is null)
            {
                return null;
            }
            var items = feature.FeatureItems.Select(FeatureItem.FromDTO).ToArray();
            var subFeatures = Traverse(feature).ToArray();
            return await Task.FromResult(Feature.FromDTO(feature, subFeatures, items));
        }

        public async Task<IEnumerable<Feature>> GetAllAsync()
        {
            var features = new List<Feature>();
            foreach (var feature in Repository.Where(e => !e.ParentId.HasValue))
            {
                var items = feature.FeatureItems.Select(FeatureItem.FromDTO).ToArray();
                var subFeatures = Traverse(feature).ToArray();
                features.Add(Feature.FromDTO(feature, subFeatures, items));
            }
            return await Task.FromResult(features);
        }
    }
}
