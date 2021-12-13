using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Internal;

namespace Rubberduck.ContentServices.Writer
{
    public class TagAssetContentWriterService : IContentWriterService<TagAsset>
    {
        private readonly RubberduckDbContext _context;

        public TagAssetContentWriterService(RubberduckDbContext context)
        {
            _context = context;
        }

        public async Task<TagAsset> CreateAsync(TagAsset entity)
        {
            if (entity.Id != default)
            {
                throw new InvalidOperationException("Cannot add an entity that already has an ID.");
            }

            var dto = TagAsset.ToDTO(entity);
            dto.DateInserted = DateTime.Now;

            await _context.TagAssets.AddAsync(dto);

            await _context.SaveChangesAsync();
            return TagAsset.FromDTO(dto);
        }

        public async Task<TagAsset> UpdateAsync(TagAsset entity)
        {
            var dto = _context.TagAssets.AsTracking().SingleOrDefault(e => e.Id == entity.Id);
            dto.DateUpdated = DateTime.Now;
            dto.DownloadUrl = entity.DownloadUrl.ToString();
            dto.Name = entity.Name;

            await _context.SaveChangesAsync();
            return TagAsset.FromDTO(dto);
        }

        public async Task DeleteAsync(TagAsset entity)
        {
            var dto = _context.TagAssets.AsTracking().SingleOrDefault(e => e.Id == entity.Id);
            _context.Remove(dto);

            await _context.SaveChangesAsync();
        }
    }
}
