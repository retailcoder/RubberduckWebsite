using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Internal;

namespace Rubberduck.ContentServices.Reader
{
    public class TagAssetContentReaderService : IContentReaderService<TagAsset>
    {
        private readonly RubberduckDbContext _context;

        public TagAssetContentReaderService(RubberduckDbContext context)
        {
            _context = context;
        }

        private IQueryable<Model.DTO.TagAssetEntity> Repository =>
            _context.TagAssets.AsNoTracking();


        public async Task<TagAsset> GetByIdAsync(int id) =>
            await Task.FromResult(Repository.Where(e => e.Id == id).Select(TagAsset.FromDTO).SingleOrDefault());

        public async Task<TagAsset> GetByEntityKeyAsync(TagAsset key) =>
            await Task.FromResult(Repository.Where(e => e.Id == key.Id || (e.TagId == key.TagId && e.Name == key.Name)).Select(TagAsset.FromDTO).SingleOrDefault());

        public async Task<IEnumerable<TagAsset>> GetAllAsync() =>
            await Task.FromResult(Repository.Select(TagAsset.FromDTO));
    }
}
