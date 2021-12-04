using System;

namespace Rubberduck.Model.Entity
{
    public class ExampleModule : IEntity
    {
        public static ExampleModule FromDTO(DTO.ExampleModule dto) => new(dto);
        public static DTO.ExampleModule ToDTO(ExampleModule entity) => new()
        {
            DateInserted = DateTime.Now,
            ExampleId = entity.ExampleId,
            Description = entity.Description,
            ModuleName = entity.ModuleName,
            ModuleType = entity.ModuleTypeId,
            HtmlContent = entity.HtmlContent
        };

        internal ExampleModule(DTO.ExampleModule dto)
        {
            Id = dto.Id;

            ExampleId = dto.ExampleId;
            ModuleName = dto.ModuleName;

            ModuleTypeId = dto.ModuleType;
            Description = dto.Description;
            HtmlContent = dto.HtmlContent;
        }

        public int Id { get; }

        public int ExampleId { get; }
        public string ModuleName { get; }

        public int ModuleTypeId { get; }
        public string Description { get; }
        public string HtmlContent { get; }

        public object Key() => new { ExampleId, ModuleName };
    }
}
