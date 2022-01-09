﻿using System;
using System.Collections.Generic;
using Rubberduck.Model.Abstract;

namespace Rubberduck.Model.Entities
{
    public class Tag : Entity
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string InstallerDownloadUrl { get; set; }
        public int InstallerDownloads { get; set; }
        public bool IsPreRelease { get; set; }

        public virtual ICollection<TagAsset> TagAssets { get; set; } = new List<TagAsset>();
    }
}
