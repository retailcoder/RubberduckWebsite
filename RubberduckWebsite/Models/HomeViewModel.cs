using System;
using System.Collections.Generic;
using System.Linq;
using Rubberduck.Model;
using Rubberduck.Model.Entities;

namespace RubberduckWebsite.Models
{
    public class AdminViewModel
    {
        public AdminViewModel(DateTime? tagsTimestamp, IEnumerable<Feature> features)
        {
            TagMetadataTimestamp = tagsTimestamp;
            Features = features.ToHashSet();
            if (tagsTimestamp.HasValue)
            {
                MillisecondsSinceLastUpdate = DateTime.UtcNow.Subtract(tagsTimestamp.Value).TotalMilliseconds;
            }
        }

        public DateTime? TagMetadataTimestamp { get; }
        public IReadOnlySet<Feature> Features { get; }
        public double? MillisecondsSinceLastUpdate { get; }
    }

    public class HomeViewModel
    {
        public HomeViewModel(IEnumerable<Tag> latestTags, IEnumerable<Feature> features)
        {
            NextTag = latestTags.SingleOrDefault(tag => tag.IsPreRelease);
            MainTag = latestTags.SingleOrDefault(tag => !tag.IsPreRelease);
            
            Features = features.Where(feature => !feature.IsHidden).ToHashSet();
            MetadataTimestamp = latestTags.Any() ? latestTags.Max(tag => tag.DateUpdated ?? tag.DateInserted) : null;
        }

        /// <summary>
        /// The release tag for the latest pre-release build.
        /// </summary>
        public Tag NextTag { get; }

        /// <summary>
        /// The release tag for the latest release build.
        /// </summary>
        public Tag MainTag { get; }

        /// <summary>
        /// All features.
        /// </summary>
        public IReadOnlySet<Feature> Features { get; }

        /// <summary>
        /// Gets the UTC timestamp for the tag and tag assets metadata. <c>null</c> if there is no tag asset metadata.
        /// </summary>
        public DateTime? MetadataTimestamp { get; }

        public SearchViewModel Search { get; }
    }
}
