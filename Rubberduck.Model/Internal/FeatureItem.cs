using System;
using System.Collections.Generic;
using System.Linq;

namespace Rubberduck.Model.Internal
{
    public class FeatureItem : EntityBase, IEquatable<FeatureItem>
    {
        public static FeatureItem FromDTO(DTO.FeatureItem dto) => new(dto);
        public static FeatureItem FromDTO(DTO.FeatureItem dto, IEnumerable<Example> examples) => new(dto, examples);
        public static DTO.FeatureItemEntity ToDTO(FeatureItem entity, bool? isDiscontinued = null, bool? isNew = null) => new()
        {
            Id = entity.Id,
            DateInserted = entity.DateInserted,
            DateUpdated = entity.DateUpdated,

            FeatureId = entity.FeatureId,
            TagAssetId = entity.TagAssetId,
            Name = entity.Name ?? string.Empty,
            Title = entity.Title ?? string.Empty,
            Description = entity.Description ?? string.Empty,
            IsHidden = entity.IsHidden,
            IsDiscontinued = isDiscontinued ?? entity.IsDiscontinued,
            IsNew = isNew ?? entity.IsNew,
            ContentUrl = entity.ContentUrl?.ToString(),
            XmlDocSourceObject = entity.RubberduckSource,
            XmlDocSummary = entity.Summary,
            XmlDocRemarks = entity.Remarks,
            XmlDocTabName = entity.TabName,
            XmlDocInfo = entity.Info,
            XmlDocMetadata = entity.Metadata
        };

        public bool Equals(FeatureItem other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other.GetType() != GetType())
            {
                return false;
            }

            return other.FeatureId == FeatureId
                && other.Name == Name;
        }

        public override bool Equals(object obj) => Equals(obj as FeatureItem);

        public override int GetHashCode() => HashCode.Combine(FeatureId, Name);

        internal FeatureItem(DTO.FeatureItem dto, IEnumerable<Example> examples)
            : this(dto)
        {
            Examples = examples?.ToArray() ?? Enumerable.Empty<Example>();
        }

        internal FeatureItem(DTO.FeatureItem dto)
            : base(dto.Id, dto.DateInserted, dto.DateUpdated)
        {
            // Natural (unique) key
            FeatureId = dto.FeatureId;
            Name = dto.Name;

            // FK
            TagAssetId = dto.TagAssetId;

            // all others...
            Title = dto.Title;
            Description = dto.Description;
            IsNew = dto.IsNew;
            IsDiscontinued = dto.IsDiscontinued;
            IsHidden = dto.IsHidden;

            // items from xmldocs...
            TabName = dto.XmlDocTabName;
            Metadata = dto.XmlDocMetadata;
            Summary = dto.XmlDocSummary;
            Info = dto.XmlDocInfo;
            Remarks = dto.XmlDocRemarks;
            RubberduckSource = dto.XmlDocSourceObject;

            if (Uri.TryCreate(dto.ContentUrl, UriKind.Absolute, out var uri))
            {
                ContentUrl = uri;
            }

            Examples = Enumerable.Empty<Example>();
        }

        public int FeatureId { get; }
        public int? TagAssetId { get; }
        public string Name { get; }
        public string Title { get; }
        public string Description { get; }
        public Uri ContentUrl { get; }
        public bool IsNew { get; }
        public bool IsDiscontinued { get; }
        public bool IsHidden { get; }

        public string TabName { get; }
        public string Metadata { get; }
        public string Summary { get; }
        public string Info { get; }
        public string Remarks { get; }
        public string RubberduckSource { get; }

        public IEnumerable<Example> Examples { get; }
    }
}
