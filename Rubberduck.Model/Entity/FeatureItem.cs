﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Rubberduck.Model.Entity
{
    public class FeatureItem : IEntity
    {
        public static FeatureItem FromDTO(DTO.FeatureItem dto) => new(dto);
        public static FeatureItem FromDTO(DTO.FeatureItem dto, IEnumerable<Example> examples) => new(dto, examples);
        public static DTO.FeatureItem ToDTO(FeatureItem entity) => new()
        {
            DateInserted = DateTime.Now,
            FeatureId = entity.FeatureId,
            Name = entity.Name,
            Title = entity.Title,
            Description = entity.Description,
            IsHidden = entity.IsHidden,
            IsNew = entity.IsNew,
            ContentUrl = entity.ContentUrl?.ToString(),
            XmlDocSourceObject = entity.RubberduckSource,
            XmlDocSummary = entity.Summary,
            XmlDocRemarks = entity.Remarks,
            XmlDocTabName = entity.TabName,
            XmlDocInfo = entity.Info,
            XmlDocMetadata = entity.Metadata
        };

        internal FeatureItem(DTO.FeatureItem dto, IEnumerable<Example> examples)
            : this(dto)
        {
            Examples = examples?.ToArray() ?? Enumerable.Empty<Example>();
        }

        internal FeatureItem(DTO.FeatureItem dto)
        {
            // Primary key
            Id = dto.Id;

            // Natural (unique) key
            FeatureId = dto.FeatureId;
            Name = dto.Name;

            // all others...
            Title = dto.Title;
            Description = dto.Description;
            IsNew = dto.IsNew;
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

        public int Id { get; }
        public int FeatureId { get; }
        public string Name { get; }
        public string Title { get; }
        public string Description { get; }
        public Uri ContentUrl { get; }
        public bool IsNew { get; }
        public bool IsHidden { get; }

        public string TabName { get; }
        public string Metadata { get; }
        public string Summary { get; }
        public string Info { get; }
        public string Remarks { get; }
        public string RubberduckSource { get; }

        public IEnumerable<Example> Examples { get; }

        public object Key() => new { FeatureId, Name };
    }
}
