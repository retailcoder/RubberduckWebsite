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
                    feature => new FeatureViewModel(feature),
                    feature => GetSubFeatures(features, feature.Id));
        }

        private static IReadOnlySet<FeatureViewModel> GetSubFeatures(IEnumerable<Feature> features, int parentId)
        {
            return features
                .Where(feature => feature.ParentId == parentId && feature.XmlDocSource is null)
                .OrderBy(feature => feature.SortOrder)
                .ThenBy(feature => feature.Name)
                .Select(e => new FeatureViewModel(e))
                .ToHashSet();
        }

        public IReadOnlyDictionary<FeatureViewModel, IReadOnlySet<FeatureViewModel>> Features { get; }
    }
}