using System.ComponentModel.DataAnnotations.Schema;

namespace Rubberduck.Model.DTO
{
    public class TagAsset : BaseDto
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public string DownloadUrl { get; set; }
    }

    [Table("TagAssets")]
    public class TagAssetEntity : TagAsset
    {
        public virtual TagEntity Tag { get; set; }
    }
}
