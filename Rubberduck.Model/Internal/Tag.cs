using System;
using System.Collections.Generic;
using System.Linq;

namespace Rubberduck.Model.Internal
{
    public class Tag : EntityBase
    {
        public static Tag FromDTO(DTO.Tag dto) => new(dto);
        public static Tag FromDTO(DTO.Tag dto, IEnumerable<TagAsset> assets) => new(dto, assets);

        public static DTO.TagEntity ToDTO(Tag entity) => new()
        {
            Id = entity.Id,
            DateInserted = entity.DateInserted,
            DateUpdated = entity.DateUpdated,

            Name = entity.Name,
            DateCreated = entity.DateCreated,
            IsPreRelease = entity.IsPreRelease,
            InstallerDownloadUrl = entity.InstallerDownloadUrl?.ToString(),
            InstallerDownloads = entity.InstallerDownloads,
            TagAssets = entity.Assets.Select(TagAsset.ToDTO).ToArray()
        };

        internal Tag(DTO.Tag dto)
            : base(dto.Id, dto.DateInserted, dto.DateUpdated)
        {
            Name = dto.Name;
            DateCreated = dto.DateCreated;

            if (Uri.TryCreate(dto.InstallerDownloadUrl, UriKind.Absolute, out var uri))
            {
                InstallerDownloadUrl = uri;
            }
            InstallerDownloads = dto.InstallerDownloads;
            IsPreRelease = dto.IsPreRelease;
            Assets = Enumerable.Empty<TagAsset>();
        }

        internal Tag(DTO.Tag dto, IEnumerable<TagAsset> assets)
            : this(dto)
        {
            Assets = assets ?? Enumerable.Empty<TagAsset>();
        }

        public string Name { get; }
        public DateTime DateCreated { get; }
        public Uri InstallerDownloadUrl { get; }
        public int InstallerDownloads { get; }
        public bool IsPreRelease { get; }
        public IEnumerable<TagAsset> Assets { get; }
    }
}
