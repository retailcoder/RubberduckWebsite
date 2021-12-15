using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Internal;
using Microsoft.EntityFrameworkCore;

namespace Rubberduck.ContentServices.Reader
{
    public class ReleaseTagContentReaderService : IContentReaderService<Tag>
    {
        private readonly RubberduckDbContext _context;

        public ReleaseTagContentReaderService(RubberduckDbContext context)
        {
            _context = context;
        }

        private IQueryable<Model.DTO.TagEntity> Repository =>
            _context.Tags.AsNoTracking().Include(e => e.TagAssets);

        public async Task<Tag> GetByIdAsync(int id)
        {
            var tag = Repository.Single(e => e.Id == id);
            var assets = tag.TagAssets.Select(TagAsset.FromDTO).ToArray();
            return await Task.FromResult(Tag.FromDTO(tag, assets));
        }

        public async Task<Tag> GetByEntityKeyAsync(Tag key)
        {
            var tag = Repository.SingleOrDefault(e => e.Id == key.Id || e.Name == key.Name);
            if (tag is null)
            {
                return null;
            }
            var assets = tag.TagAssets.Select(TagAsset.FromDTO).ToArray();
            return await Task.FromResult(Tag.FromDTO(tag, assets));
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            var tags = new List<Tag>();
            foreach (var tag in Repository)
            {
                var assets = tag.TagAssets.Select(TagAsset.FromDTO).ToArray();
                tags.Add(Tag.FromDTO(tag, assets));
            }
            return await Task.FromResult(tags);
        }
    }
}
