using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Internal;

namespace Rubberduck.ContentServices.Writer
{
    public class FeatureContentWriterService : IContentWriterService<Feature>
    {
        private readonly RubberduckDbContext _context;

        public FeatureContentWriterService(RubberduckDbContext context)
        {
            _context = context;
        }

        public async Task<Feature> CreateAsync(Feature entity)
        {
            if (entity.Id != default)
            {
                throw new InvalidOperationException("Cannot add an entity that already has an ID.");
            }

            var dto = Feature.ToDTO(entity);
            dto.DateInserted = DateTime.Now;

            await _context.Features.AddAsync(dto);

            await _context.SaveChangesAsync();
            return Feature.FromDTO(dto);
        }

        public async Task<Feature> UpdateAsync(Feature entity)
        {
            var dto = _context.Features.AsTracking().SingleOrDefault(e => e.Id == entity.Id);
            dto.DateUpdated = DateTime.Now;
            dto.ContentUrl = entity.ContentUrl.ToString();
            dto.Description = entity.Description;
            dto.IsHidden = entity.IsHidden;
            dto.IsNew = entity.IsNew;
            dto.Name = entity.Name;
            dto.ParentId = entity.ParentId;
            dto.SortOrder = entity.SortOrder;
            dto.Title = entity.Title;
            dto.XmlDocSource = entity.XmlDocSource;

            await _context.SaveChangesAsync();
            return Feature.FromDTO(dto);
        }

        public async Task DeleteAsync(Feature entity)
        {
            var dto = _context.Features.AsTracking().SingleOrDefault(e => e.Id == entity.Id);
            _context.Remove(dto);

            await _context.SaveChangesAsync();
        }
    }
}
