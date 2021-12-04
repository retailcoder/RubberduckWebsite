using System.ComponentModel.DataAnnotations.Schema;

namespace Rubberduck.Model.DTO
{
    [Table("TagAssets")]
    public class TagAsset : BaseDto
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public string DownloadUrl { get; set; }
    }
}
