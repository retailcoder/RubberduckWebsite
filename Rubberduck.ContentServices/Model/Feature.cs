using System;
using System.Collections.Generic;
using System.Linq;
using Rubberduck.Model;
using Rubberduck.Model.Abstract;
using PublicModel = Rubberduck.Model.Entities;

namespace Rubberduck.ContentServices.Model
{
    public class Feature : Entity, IInternalModel<PublicModel.Feature>
    {
        public Feature() { }
        public Feature(PublicModel.Feature model)
        {
            Id = model.Id;
            DateInserted = model.DateInserted;
            DateUpdated = model.DateUpdated;

            ParentId = model.ParentId;
            Name = model.Name;
            Title = model.Title;
            ElevatorPitch = model.ElevatorPitch;
            Description = model.Description;
            IsNew = model.IsNew;
            IsHidden = model.IsHidden;
            SortOrder = model.SortOrder;
            XmlDocSource = model.XmlDocSource;

            ParentFeature = model.ParentFeature is null ? null : new Feature(model.ParentFeature);
            SubFeatures = model.SubFeatures.Select(m => new Feature(m)).ToList();
            FeatureItems = model.FeatureItems.Select(m => new FeatureItem(m)).ToList();
        }

        /// <summary>
        /// Refers to the <c>Id</c> of the parent feature if applicable; <c>null</c> otherwise.
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// A short, unique identifier string.
        /// </summary>
        /// <remarks>
        /// While values are PascalCased, this value should be URL-encoded regardless, if used as part of a valid URI.
        /// </remarks>
        public string Name { get; set; }
        /// <summary>
        /// The name of the feature.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// A short paragraph (2-3 sentences) that describes the feature.
        /// </summary>
        public string ElevatorPitch { get; set; }
        /// <summary>
        /// Markdown content for a detailed description of the feature.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Indicates whether this feature exists in the [next] branch but not yet in the [main] one.
        /// </summary>
        public bool IsNew { get; set; }
        /// <summary>
        /// Indicates whether this feature should be shown.
        /// </summary>
        public bool IsHidden { get; set; }
        /// <summary>
        /// An integer value that is used for sorting the features.
        /// </summary>
        /// <remarks>
        /// Client should use the <c>Name</c> as a 2nd sort level.
        /// </remarks>
        public int SortOrder { get; set; }
        /// <summary>
        /// The name of the .xml file this record comes from.
        /// </summary>
        public string XmlDocSource { get; set; }

        public virtual Feature ParentFeature { get; set; }
        public virtual ICollection<Feature> SubFeatures { get; set; } = new List<Feature>();
        public virtual ICollection<FeatureItem> FeatureItems { get; set; } = new List<FeatureItem>();

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            var other = obj as Feature;
            {
                return other.Name == Name;
            }
        }
        public override int GetHashCode() => HashCode.Combine(Name);

        public PublicModel.Feature ToPublicModel()
        {
            return new PublicModel.Feature
            {
                Id = this.Id,
                DateInserted = this.DateInserted,
                DateUpdated = this.DateUpdated,

                ParentId = this.ParentId,

                Name = this.Name,
                Title = this.Title,
                ElevatorPitch = this.ElevatorPitch,
                Description = this.Description,
                IsNew = this.IsNew,
                IsHidden = this.IsHidden,
                SortOrder = this.SortOrder,
                XmlDocSource = this.XmlDocSource,

                SubFeatures = this.SubFeatures.Select(e => e.ToPublicModel()).ToList(),
                FeatureItems = this.FeatureItems.Select(e => e.ToPublicModel()).ToList()
            };
        }

        public SearchResultViewModel AsSearchResult(string search)
        {
            var match = this.Title.Contains(search, StringComparison.InvariantCultureIgnoreCase) ? this.Title
                : this.ElevatorPitch.Contains(search, StringComparison.InvariantCultureIgnoreCase) ? this.ElevatorPitch
                : this.Description.Contains(search, StringComparison.InvariantCultureIgnoreCase) ? this.Description
                : null;
            if (match is null)
            {
                return null;
            }

            if (match.Length > 300)
            {
                var index = match.IndexOf(search, StringComparison.InvariantCultureIgnoreCase);
                if (index <= 150)
                {
                    match = match.Substring(0, 300);
                }
                else
                {
                    match = "[...] " + match.Substring(index - 80, Math.Min(300, match.Length - index - 80)) + " [...]";
                }
            }

            return new SearchResultViewModel
            {
                Title = this.Title,
                Url = $"Features/{this.Name}",
                Excerpt = match
            };
        }
    }
}
