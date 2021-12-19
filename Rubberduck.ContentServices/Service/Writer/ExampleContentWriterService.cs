using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Internal;

namespace Rubberduck.ContentServices.Writer
{
    public class ExampleContentWriterService : IContentWriterService<Example>
    {
        private readonly RubberduckDbContext _context;

        public ExampleContentWriterService(RubberduckDbContext context)
        {
            _context = context;
        }

        public async Task<Example> CreateAsync(Example entity)
        {
            if (entity.Id != default)
            {
                throw new InvalidOperationException("Cannot add an entity that already has an ID.");
            }

            var dto = Example.ToDTO(entity);
            dto.DateInserted = DateTime.UtcNow;

            await _context.Examples.AddAsync(dto);
            await _context.SaveChangesAsync();
            return Example.FromDTO(dto);
        }

        public async Task<Example> UpdateAsync(Example entity) => await Task.Run(() =>
        {
            var dto = _context.Examples.AsTracking().SingleOrDefault(e => e.Id == entity.Id);
            if (IsDirty(entity, dto))
            {
                dto.DateUpdated = DateTime.UtcNow;
                dto.Description = entity.Description;
                dto.SortOrder = entity.SortOrder;
            }
            return Example.FromDTO(dto);
        });

        private static bool IsDirty(Example model, Model.DTO.ExampleEntity dto) => 
            model.Description != dto.Description || model.SortOrder != dto.SortOrder;

        public async Task DeleteAsync(Example entity) => await Task.Run(() =>
        {
            var dto = _context.Examples.AsTracking().SingleOrDefault(e => e.Id == entity.Id);
            _context.Remove(dto);
        });
    }
}
