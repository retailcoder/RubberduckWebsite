using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Internal;

namespace Rubberduck.ContentServices.Writer
{
    public class TagContentWriterService : IContentWriterService<Tag>
    {
        private readonly RubberduckDbContext _context;

        public TagContentWriterService(RubberduckDbContext context)
        {
            _context = context;
        }

        public async Task<Tag> CreateAsync(Tag entity)
        {
            if (entity.Id != default)
            {
                throw new InvalidOperationException("Cannot add an entity that already has an ID.");
            }

            var dto = Tag.ToDTO(entity);
            dto.DateInserted = DateTime.Now;

            await _context.Tags.AddAsync(dto);

            await _context.SaveChangesAsync();
            return Tag.FromDTO(dto);
        }

        public async Task<Tag> UpdateAsync(Tag entity)
        {
            var dto = _context.Tags.AsTracking().SingleOrDefault(e => e.Id == entity.Id);
            dto.DateUpdated = DateTime.Now;
            dto.InstallerDownloads = entity.InstallerDownloads;

            await _context.SaveChangesAsync();
            return Tag.FromDTO(dto);
        }

        public async Task DeleteAsync(Tag entity)
        {
            var dto = _context.Tags.AsTracking().SingleOrDefault(e => e.Id == entity.Id);
            _context.Remove(dto);

            await _context.SaveChangesAsync();
        }
    }
}
