using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubberduck.Model.DTO
{
    public class FeatureItem : BaseDto
    {
        public int FeatureId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ContentUrl { get; set; }
        public bool IsNew { get; set; }
        public bool IsDiscontinued { get; set; }
        public bool IsHidden { get; set; }
        public int? TagAssetId { get; set; }
        public string XmlDocSourceObject { get; set; }
        public string XmlDocTabName { get; set; }
        public string XmlDocMetadata { get; set; }
        public string XmlDocSummary { get; set; }
        public string XmlDocInfo { get; set; }
        public string XmlDocRemarks { get; set; }
    }

    [Table("FeatureItems")]
    public class FeatureItemEntity : FeatureItem
    {
        public virtual FeatureEntity Feature { get; set; }
        public virtual TagAssetEntity TagAsset { get; set; }
        public virtual ICollection<ExampleEntity> Examples { get; set; } = new List<ExampleEntity>();
    }
}
