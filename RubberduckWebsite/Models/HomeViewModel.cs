using System;
using System.Collections.Generic;
using System.Linq;
using Rubberduck.Model.Entities;

namespace RubberduckWebsite.Models
{
    public class HomeViewModel
    {
        public HomeViewModel(IEnumerable<Tag> latestTags, IEnumerable<Feature> features)
        {
            NextTag = latestTags.SingleOrDefault(tag => tag.IsPreRelease);
            MainTag = latestTags.SingleOrDefault(tag => !tag.IsPreRelease);
            
            Features = features.ToHashSet();
            MetadataTimestamp = latestTags.Max(tag => tag.DateUpdated ?? tag.DateInserted);
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
        /// Gets the UTC timestamp for the tag and tag assets metadata.
        /// </summary>
        public DateTime MetadataTimestamp { get; }
    }
}
