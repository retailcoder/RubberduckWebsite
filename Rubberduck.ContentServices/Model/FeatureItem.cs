using System;
using System.Collections.Generic;
using System.Linq;
using Rubberduck.Model.Abstract;
using PublicModel = Rubberduck.Model.Entities;

namespace Rubberduck.ContentServices.Model
{
    public class FeatureItem : Entity, IInternalModel<PublicModel.FeatureItem>
    {
        public FeatureItem() { }
        public FeatureItem(PublicModel.FeatureItem model)
        {
            Id = model.Id;
            DateInserted = model.DateInserted;
            DateUpdated = model.DateUpdated;

            FeatureId = model.FeatureId;
            Name = model.Name;
            Title = model.Title;
            Description = model.Description;
            IsNew = model.IsNew;
            IsDiscontinued = model.IsDiscontinued;
            IsHidden = model.IsHidden;
            TagAssetId = model.TagAssetId;
            XmlDocSourceObject = model.XmlDocSourceObject;
            XmlDocTabName = model.XmlDocTabName;
            XmlDocMetadata = model.XmlDocMetadata;
            XmlDocSummary = model.XmlDocSummary;
            XmlDocInfo = model.XmlDocInfo;
            XmlDocRemarks = model.XmlDocRemarks;

            Feature = model.Feature is null ? null : new Feature(model.Feature);
            TagAsset = model.TagAsset is null ? null : new TagAsset(model.TagAsset);
            Examples = model.Examples.Select(m => new Example(m)).ToList();
        }

        public int FeatureId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsNew { get; set; }
        public bool IsDiscontinued { get; set; }
        public bool IsHidden { get; set; }
        public int? TagAssetId { get; set; }
        public string XmlDocSourceObject { get; set; }
        public string XmlDocTabName { get; set; }
        public string XmlDocMetadata { get; set; }
        public string XmlDocSummary { get; set; }
        public string XmlDocInfo { get; set; }
        public string XmlDocRemarks { get; set; }

        public virtual Feature Feature { get; set; }
        public virtual TagAsset TagAsset { get; set; }
        public virtual ICollection<Example> Examples { get; set; } = new List<Example>();

        public PublicModel.FeatureItem ToPublicModel()
        {
            return new PublicModel.FeatureItem
            {
                Id = this.Id,
                DateInserted = this.DateInserted,
                DateUpdated = this.DateUpdated,

                FeatureId = this.FeatureId,
                TagAssetId = this.TagAssetId,

                Name = this.Name,
                Title = this.Title,
                Description = this.Description,
                IsNew = this.IsNew,
                IsDiscontinued = this.IsDiscontinued,
                IsHidden = this.IsHidden,

                XmlDocSourceObject = this.XmlDocSourceObject,
                XmlDocTabName = this.XmlDocTabName,
                XmlDocSummary = this.XmlDocSummary,
                XmlDocInfo = this.XmlDocInfo,
                XmlDocRemarks = this.XmlDocRemarks,
                XmlDocMetadata = this.XmlDocMetadata,

                TagAsset = this.TagAsset?.ToPublicModel(),
                Examples = this.Examples.Select(e => e.ToPublicModel()).ToList()
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj is FeatureItem other)
            {
                return other.Name == this.Name;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
