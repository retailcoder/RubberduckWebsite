using System;

namespace Rubberduck.Model.Entity
{
    public class TagAsset : IEntity
    {
        public static TagAsset FromDTO(DTO.TagAsset dto) => new(dto);
        public static DTO.TagAsset ToDTO(TagAsset entity) => new()
        {
            Id = entity.Id,
            TagId = entity.TagId,
            Name = entity.Name,
            DownloadUrl = entity.DownloadUrl?.ToString()
        };

        internal TagAsset(DTO.TagAsset dto)
        {
            Id = dto.Id;
            TagId = dto.TagId;
            Name = dto.Name;
            if (Uri.TryCreate(dto.DownloadUrl, UriKind.Absolute, out var uri))
            {
                DownloadUrl = uri;
            }
        }


        public int Id { get; }

        public int TagId { get; }
        public string Name { get; }
        public Uri DownloadUrl { get; }

        public object Key() => new { Id };
    }
}
