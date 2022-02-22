using System.Collections.Generic;
using System.Linq;
using Rubberduck.Model.Entities;

namespace RubberduckWebsite.Models
{
    public class FeaturesViewModel
    {
        public FeaturesViewModel(IEnumerable<Feature> features)
        {
            Features = features
                .Where(feature => feature.ParentId is null)
                .OrderBy(feature => feature.SortOrder)
                .ThenBy(feature => feature.Name)
                .ToDictionary(
                    feature => feature,
                    feature => GetSubFeatures(features, feature.Id));
        }

        private static IReadOnlySet<Feature> GetSubFeatures(IEnumerable<Feature> features, int parentId)
        {
            return features
                .Where(feature => feature.ParentId == parentId && feature.XmlDocSource is null)
                .OrderBy(feature => feature.SortOrder)
                .ThenBy(feature => feature.Name)
                .ToHashSet();
        }

        public IReadOnlyDictionary<Feature, IReadOnlySet<Feature>> Features { get; }
    }
}