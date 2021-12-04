using System.ComponentModel.DataAnnotations.Schema;

namespace Rubberduck.Model.DTO
{
    [Table("ExampleModules")]
    public class ExampleModule : BaseDto
    {
        public int ExampleId { get; set; }
        public string ModuleName { get; set; }
        public int ModuleType { get; set; }
        public string Description { get; set; }
        public string HtmlContent { get; set; }
    }
}
