using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Rubberduck.Model.DTO
{
    [Table("Tags")]
    public class Tag : BaseDto
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string InstallerDownloadUrl { get; set; }
        public int InstallerDownloads { get; set; }
        public bool IsPreRelease { get; set; }

        public IEnumerable<TagAsset> TagAssets { get; set; } = Enumerable.Empty<TagAsset>();
    }
}
