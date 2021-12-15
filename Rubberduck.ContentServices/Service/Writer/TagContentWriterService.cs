using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Internal;

namespace Rubberduck.ContentServices.Writer
{
    public class TagContentWriterService : IContentWriterService<Tag>
    {
        private readonly RubberduckDbContext _context;
        private readonly ILogger _logger;

        public TagContentWriterService(RubberduckDbContext context, ILogger<TagContentWriterService> logger)
        {
            _context = context;
            _logger = logger;
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
            _logger.LogTrace($"Added tag {dto.Name} (created {dto.DateCreated}, {dto.InstallerDownloads} installer downloads): ID {dto.Id}");
            return Tag.FromDTO(dto);
        }

        public async Task<Tag> UpdateAsync(Tag entity) => await Task.Run(() =>
        {
            var dto = _context.Tags.AsTracking().SingleOrDefault(e => e.Id == entity.Id || e.Name == entity.Name);
            if (IsDirty(entity, dto))
            {
                dto.DateUpdated = DateTime.Now;
                dto.IsPreRelease = entity.IsPreRelease;
                dto.InstallerDownloads = entity.InstallerDownloads;
                _logger.LogInformation($"Tag {dto.Name} has had {entity.InstallerDownloads - dto.InstallerDownloads} installer downloads since last update ({dto.DateUpdated ?? dto.DateInserted}).");
            }
            return Tag.FromDTO(dto);
        });

        private static bool IsDirty(Tag model, Model.DTO.TagEntity dto) =>
            model.IsPreRelease != dto.IsPreRelease
            || model.InstallerDownloads != dto.InstallerDownloads;

        public async Task DeleteAsync(Tag entity) => await Task.Run(() =>
        {
            var dto = _context.Tags.AsTracking().SingleOrDefault(e => e.Id == entity.Id);
            _context.Remove(dto);
            _logger.LogTrace($"Removed tag {dto.Name} (created {dto.DateCreated}, {dto.InstallerDownloads} installer downloads).");
        });
    }
}
