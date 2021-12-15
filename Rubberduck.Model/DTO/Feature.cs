using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rubberduck.Model.DTO
{
    public class Feature : BaseDto
    {
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ContentUrl { get; set; }
        public bool IsNew { get; set; }
        public bool IsHidden { get; set; }
        public int SortOrder { get; set; }
        public string XmlDocSource { get; set; }
    }

    [Table("Features")]
    public class FeatureEntity : Feature
    {
        public virtual FeatureEntity ParentFeature { get; set; }
        public virtual ICollection<FeatureEntity> SubFeatures { get; set; } = new List<FeatureEntity>();
        public virtual ICollection<FeatureItemEntity> FeatureItems { get; set; } = new List<FeatureItemEntity>();
    }
}
