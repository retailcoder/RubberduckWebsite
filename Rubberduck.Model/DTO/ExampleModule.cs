using System.ComponentModel.DataAnnotations.Schema;

namespace Rubberduck.Model.DTO
{
    public class ExampleModule : BaseDto
    {
        public int ExampleId { get; set; }
        public string ModuleName { get; set; }
        public int ModuleType { get; set; }
        public string Description { get; set; }
        public string HtmlContent { get; set; }
    }

    [Table("ExampleModules")]
    public class ExampleModuleEntity : ExampleModule
    {
        public virtual ExampleEntity Example { get; set; }
    }
}
