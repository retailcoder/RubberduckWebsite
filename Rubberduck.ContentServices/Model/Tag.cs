using System;
using System.Collections.Generic;
using System.Linq;
using Rubberduck.Model.Abstract;
using PublicModel = Rubberduck.Model.Entities;

namespace Rubberduck.ContentServices.Model
{
    public class Tag : Entity, IInternalModel<PublicModel.Tag>
    {
        public Tag() { }
        public Tag(PublicModel.Tag model)
        {
            Id = model.Id;
            DateInserted = model.DateInserted;
            DateUpdated = model.DateUpdated;

            Name = model.Name;
            DateCreated = model.DateCreated;
            InstallerDownloads = model.InstallerDownloads;
            InstallerDownloadUrl = model.InstallerDownloadUrl;
            IsPreRelease = model.IsPreRelease;

            TagAssets = model.TagAssets.Select(m => new TagAsset(m)).ToList();
        }

        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string InstallerDownloadUrl { get; set; }
        public int InstallerDownloads { get; set; }
        public bool IsPreRelease { get; set; }

        public virtual ICollection<TagAsset> TagAssets { get; set; } = new List<TagAsset>();

        public PublicModel.Tag ToPublicModel()
        {
            return new PublicModel.Tag
            {
                Id = this.Id,
                DateInserted = this.DateInserted,
                DateUpdated = this.DateUpdated,

                Name = this.Name,
                DateCreated = this.DateCreated,
                InstallerDownloads = this.InstallerDownloads,
                InstallerDownloadUrl = this.InstallerDownloadUrl,
                IsPreRelease = this.IsPreRelease,

                TagAssets = this.TagAssets.Select(e => e.ToPublicModel()).ToList()
            };
        }
    }
}
