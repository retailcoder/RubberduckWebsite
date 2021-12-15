using System;

namespace Rubberduck.Model.Internal
{
    public class TagAsset : EntityBase
    {
        public static TagAsset FromDTO(DTO.TagAsset dto) => new(dto);
        public static DTO.TagAssetEntity ToDTO(TagAsset entity) => new()
        {
            Id = entity.Id,
            DateInserted = entity.DateInserted,
            DateUpdated = entity.DateUpdated,

            TagId = entity.TagId,
            Name = entity.Name,
            DownloadUrl = entity.DownloadUrl?.ToString()
        };

        internal TagAsset(DTO.TagAsset dto)
            : base(dto.Id, dto.DateInserted, dto.DateUpdated)
        {
            TagId = dto.TagId;
            Name = dto.Name;
            if (Uri.TryCreate(dto.DownloadUrl, UriKind.Absolute, out var uri))
            {
                DownloadUrl = uri;
            }
        }

        public int TagId { get; }
        public string Name { get; }
        public Uri DownloadUrl { get; }
    }
}
