using System.ComponentModel.DataAnnotations.Schema;

namespace Rubberduck.Model.DTO
{
    [Table("Examples")]
    public class Example : BaseDto
    {
        public int FeatureItemId { get; set; }
        public int SortOrder { get; set; }
        public string Description { get; set; }
    }
}
