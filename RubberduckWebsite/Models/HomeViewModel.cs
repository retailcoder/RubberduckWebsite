using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rubberduck.Model.DTO;

namespace RubberduckWebsite.Models
{
    public class HomeViewModel
    {
        public HomeViewModel(IEnumerable<Tag> latestTags, IEnumerable<Feature> features)
        {
            NextTag = latestTags.SingleOrDefault(tag => tag.IsPreRelease);
            MainTag = latestTags.SingleOrDefault(tag => !tag.IsPreRelease);
            
            Features = features.ToHashSet();
            var featuresByName = features.ToDictionary(feature => feature.Name);

            HighlightFeatures = new[]
            {
                featuresByName["CodeInspections"],
                featuresByName["Refactorings"],
                featuresByName["UnitTesting"],
                //featuresByName["Navigation"],
            }.ToHashSet();
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
        /// A subset of features highlighted in a dedicated section of the page.
        /// </summary>
        public IReadOnlySet<Feature> HighlightFeatures { get; }
    }
}
