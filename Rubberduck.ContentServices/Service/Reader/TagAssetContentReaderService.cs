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
            await Task.FromResult(TagAsset.FromDTO(Repository.Single(e => e.Id == id)));

        public async Task<TagAsset> GetByEntityKeyAsync(TagAsset key) =>
            await Task.FromResult(TagAsset.FromDTO(Repository.Single(e => e.Id == key.Id)));

        public async Task<IEnumerable<TagAsset>> GetAllAsync() =>
            await Task.FromResult(Repository.Select(TagAsset.FromDTO));
    }
}
