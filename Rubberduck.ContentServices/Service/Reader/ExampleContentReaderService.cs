using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Internal;

namespace Rubberduck.ContentServices.Reader
{
    public class ExampleContentReaderService : IContentReaderService<Example>
    {
        private readonly RubberduckDbContext _context;

        public ExampleContentReaderService(RubberduckDbContext context)
        {
            _context = context;
        }

        private IQueryable<Model.DTO.ExampleEntity> Repository => 
            _context.Examples.AsNoTracking().Include(e => e.Modules);

        public async Task<Example> GetByIdAsync(int id)
        {
            var example = Repository.Single(e => e.Id == id);
            var modules = example.Modules.Select(ExampleModule.FromDTO);
            return await Task.FromResult(Example.FromDTO(example, modules));
        }

        public async Task<Example> GetByEntityKeyAsync(Example key)
        {
            var example = Repository.SingleOrDefault(e => e.FeatureItemId == key.FeatureItemId && e.SortOrder == key.SortOrder);
            var modules = example.Modules.Select(ExampleModule.FromDTO);
            return await Task.FromResult(Example.FromDTO(example, modules));
        }

        public async Task<IEnumerable<Example>> GetAllAsync()
        {
            var examples = new List<Example>();
            foreach (var example in Repository)
            {
                var modules = example.Modules.Select(ExampleModule.FromDTO);
                examples.Add(Example.FromDTO(example, modules));
            }
            return await Task.FromResult(examples);
        }
    }
}
