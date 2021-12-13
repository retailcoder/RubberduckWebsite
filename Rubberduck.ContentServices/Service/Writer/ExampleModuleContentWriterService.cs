using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Internal;

namespace Rubberduck.ContentServices.Writer
{
    public class ExampleModuleContentWriterService : IContentWriterService<ExampleModule>
    {
        private readonly RubberduckDbContext _context;

        public ExampleModuleContentWriterService(RubberduckDbContext context)
        {
            _context = context;
        }

        public async Task<ExampleModule> CreateAsync(ExampleModule entity)
        {
            if (entity.Id != default)
            {
                throw new InvalidOperationException("Cannot add an entity that already has an ID.");
            }

            var dto = ExampleModule.ToDTO(entity);
            dto.DateInserted = DateTime.Now;

            await _context.ExampleModules.AddAsync(dto);

            await _context.SaveChangesAsync();
            return ExampleModule.FromDTO(dto);
        }

        public async Task<ExampleModule> UpdateAsync(ExampleModule entity)
        {
            var dto = _context.ExampleModules.AsTracking().SingleOrDefault(e => e.Id == entity.Id);
            dto.DateUpdated = DateTime.Now;
            dto.Description = entity.Description;
            dto.HtmlContent = entity.HtmlContent;
            dto.ModuleType = (int)entity.ModuleType;

            await _context.SaveChangesAsync();
            return ExampleModule.FromDTO(dto);
        }

        public async Task DeleteAsync(ExampleModule entity)
        {
            var dto = _context.ExampleModules.AsTracking().SingleOrDefault(e => e.Id == entity.Id);
            _context.Remove(dto);

            await _context.SaveChangesAsync();
        }
    }
}
