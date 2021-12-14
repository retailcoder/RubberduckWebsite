using System;
using System.ComponentModel;

namespace Rubberduck.Model.Internal
{
    public enum ExampleModuleType
    {
        None = 0,
        [Description("(Any)")]Any,
        [Description("Class Module")]ClassModule,
        [Description("Document Module")]DocumentModule,
        [Description("Interface Module")]InterfaceModule,
        [Description("Predeclared Class")]PredeclaredClass,
        [Description("Standard Module")]StandardModule,
        [Description("UserForm Module")]UserFormModule
    }

    public class ExampleModule : EntityBase
    {
        public static ExampleModule FromDTO(DTO.ExampleModule dto) => new(dto);
        public static DTO.ExampleModuleEntity ToDTO(ExampleModule entity) => new()
        {
            Id = entity.Id,
            DateInserted = entity.DateInserted,
            DateUpdated = entity.DateUpdated,

            ExampleId = entity.ExampleId,
            Description = entity.Description,
            ModuleName = entity.ModuleName,
            ModuleType = (int)entity.ModuleType,
            HtmlContent = entity.HtmlContent
        };

        internal ExampleModule(DTO.ExampleModule dto)
            : base(dto.Id, dto.DateInserted, dto.DateUpdated)
        {

            ExampleId = dto.ExampleId;
            ModuleName = dto.ModuleName;

            ModuleType = (ExampleModuleType)dto.ModuleType;
            Description = dto.Description;
            HtmlContent = dto.HtmlContent;
        }


        public int ExampleId { get; }
        public string ModuleName { get; }

        public ExampleModuleType ModuleType { get; }
        public string Description { get; }
        public string HtmlContent { get; }
    }
}
