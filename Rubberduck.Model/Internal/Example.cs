using System;
using System.Collections.Generic;
using System.Linq;

namespace Rubberduck.Model.Internal
{
    public class Example : IEntity
    {
        public static Example FromDTO(DTO.Example dto) => new(dto);
        public static Example FromDTO(DTO.Example dto, IEnumerable<ExampleModule> modules) => new(dto, modules);
        public static DTO.ExampleEntity ToDTO(Example entity) => new()
        {
            DateInserted = DateTime.Now,
            FeatureItemId = entity.FeatureItemId,
            Description = entity.Description,
            Modules = entity.Modules.Select(e => ExampleModule.ToDTO(e)).ToList()
        };

        internal Example(DTO.Example dto)
        {
            Id = dto.Id;
            FeatureItemId = dto.FeatureItemId;
            Description = dto.Description;
            SortOrder = dto.SortOrder;
            Modules = Enumerable.Empty<ExampleModule>();
        }

        internal Example(DTO.Example dto, IEnumerable<ExampleModule> modules)
            : this(dto)
        {
            Modules = modules ?? Enumerable.Empty<ExampleModule>();
        }

        public int Id { get; }
        public int FeatureItemId { get; }
        public int SortOrder { get; }
        public string Description { get; }
        
        public IEnumerable<ExampleModule> Modules { get; }
    }
}
