using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rubberduck.Model.DTO
{
    public class Example : BaseDto
    {
        public int FeatureItemId { get; set; }
        public int SortOrder { get; set; }
        public string Description { get; set; }
    }

    [Table("Examples")]
    public class ExampleEntity : Example
    {
        public virtual FeatureItemEntity FeatureItem { get; set; }
        public virtual ICollection<ExampleModuleEntity> Modules { get; set; }
    }
}
