using System;
using System.Collections.Generic;
using System.Linq;

namespace Rubberduck.Model.Internal
{
    public class Feature : EntityBase
    {
        public static Feature FromDTO(DTO.Feature dto) => new(dto);
        public static Feature FromDTO(DTO.Feature dto, IEnumerable<Feature> subFeatures) => new(dto, subFeatures);
        public static Feature FromDTO(DTO.Feature dto, IEnumerable<FeatureItem> items) => new(dto, items);
        public static Feature FromDTO(DTO.Feature dto, IEnumerable<Feature> subFeatures, IEnumerable<FeatureItem> items) => new(dto, subFeatures, items);

        public static DTO.FeatureEntity ToDTO(Feature entity) => new()
        {
            Id = entity.Id,
            DateInserted = entity.DateInserted,
            DateUpdated = entity.DateUpdated,

            ParentId = entity.ParentId,
            Name = entity.Name ?? string.Empty,
            Title = entity.Title ?? string.Empty,
            Description = entity.Description ?? string.Empty,
            IsHidden = entity.IsHidden,
            IsNew = entity.IsNew,
            SortOrder = entity.SortOrder,
            ContentUrl = entity.ContentUrl?.ToString(),
            XmlDocSource = entity.XmlDocSource,
            FeatureItems = entity.Items.Select(e => FeatureItem.ToDTO(e)).ToList(),
            SubFeatures = entity.SubFeatures.Select(ToDTO).ToList()
        };

        internal Feature(DTO.Feature dto)
            : base(dto.Id, dto.DateInserted, dto.DateUpdated)
        {
            ParentId = dto.ParentId;

            Name = dto.Name;
            Title = dto.Title;
            Description = dto.Description;
            IsNew = dto.IsNew;
            IsHidden = dto.IsHidden;
            SortOrder = dto.SortOrder;
            XmlDocSource = dto.XmlDocSource;

            if (Uri.TryCreate(dto.ContentUrl, UriKind.Absolute, out var uri))
            {
                ContentUrl = uri;
            }

            Items = Enumerable.Empty<FeatureItem>();
            SubFeatures = Enumerable.Empty<Feature>();
        }

        internal Feature(DTO.Feature dto, IEnumerable<Feature> subFeatures)
            : this(dto)
        {
            SubFeatures = subFeatures?.ToArray() ?? Enumerable.Empty<Feature>();
        }

        internal Feature(DTO.Feature dto, IEnumerable<FeatureItem> items)
            : this(dto)
        {
            Items = items?.ToArray() ?? Enumerable.Empty<FeatureItem>();
        }
        internal Feature(DTO.Feature dto, IEnumerable<Feature> subFeatures, IEnumerable<FeatureItem> items)
            : this(dto)
        {
            SubFeatures = subFeatures?.ToArray) ?? Enumerable.Empty<Feature>();
            Items = items?.ToArray() ?? Enumerable.Empty<FeatureItem>();
        }

        public int? ParentId { get; }
        public string Name { get; }
        public string Title { get; }
        public string Description { get; }
        public Uri ContentUrl { get; }
        public bool IsNew { get; }
        public bool IsHidden { get; }
        public int SortOrder { get; }
        public string XmlDocSource { get; }
        public IEnumerable<Feature> SubFeatures { get; }
        public IEnumerable<FeatureItem> Items { get; }
    }
}
