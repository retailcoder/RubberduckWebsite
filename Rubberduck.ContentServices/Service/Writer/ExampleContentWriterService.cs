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
            dto.DateInserted = DateTime.Now;

            await _context.Examples.AddAsync(dto);

            await _context.SaveChangesAsync();
            return Example.FromDTO(dto);
        }

        public async Task<Example> UpdateAsync(Example entity)
        {
            var dto = _context.Examples.AsTracking().SingleOrDefault(e => e.Id == entity.Id);
            dto.DateUpdated = DateTime.Now;
            dto.Description = entity.Description;
            dto.SortOrder = entity.SortOrder;

            await _context.SaveChangesAsync();
            return Example.FromDTO(dto);
        }

        public async Task DeleteAsync(Example entity)
        {
            var dto = _context.Examples.AsTracking().SingleOrDefault(e => e.Id == entity.Id);
            _context.Remove(dto);

            await _context.SaveChangesAsync();
        }
    }
}
