using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Internal;
using Microsoft.EntityFrameworkCore;

namespace Rubberduck.ContentServices.Reader
{
    public class FeatureItemContentReaderService : IContentReaderService<FeatureItem>
    {
        private readonly RubberduckDbContext _context;

        public FeatureItemContentReaderService(RubberduckDbContext context)
        {
            _context = context;
        }

        private IQueryable<Model.DTO.FeatureItem> Repository =>
            _context.FeatureItems.AsNoTracking();

        public async Task<FeatureItem> GetByIdAsync(int id) =>
            await Task.FromResult(FeatureItem.FromDTO(Repository.Single(e => e.Id == id)));

        public async Task<FeatureItem> GetByEntityKeyAsync(FeatureItem key) =>
            await Task.FromResult(FeatureItem.FromDTO(Repository.Single(e => e.FeatureId == key.FeatureId && e.Name == key.Name)));

        public async Task<IEnumerable<FeatureItem>> GetAllAsync() =>
            await Task.FromResult(Repository.Select(FeatureItem.FromDTO).ToList());
    }
}
