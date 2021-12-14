using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rubberduck.Model.DTO
{
    public class Tag : BaseDto
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string InstallerDownloadUrl { get; set; }
        public int InstallerDownloads { get; set; }
        public bool IsPreRelease { get; set; }
    }

    [Table("Tags")]
    public class TagEntity : Tag
    {
        public virtual ICollection<TagAssetEntity> TagAssets { get; set; } = new List<TagAssetEntity>();
    }
}
