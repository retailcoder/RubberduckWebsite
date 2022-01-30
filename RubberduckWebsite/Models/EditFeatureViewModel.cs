using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rubberduck.Model.Entities;

namespace RubberduckWebsite.Models
{
    public class EditFeatureItemViewModel
    {
        private readonly FeatureItem _item;
        private readonly IDictionary<string, Feature> _features;

        public EditFeatureItemViewModel() : this(new(), Enumerable.Empty<Feature>()) { }

        public EditFeatureItemViewModel(FeatureItem item, IEnumerable<Feature> features)
        {
            _item = item;
            _features = features.ToDictionary(e => e.Name, e => e);
        }


        public bool IsPersisted => _item.Id != default;
        public IEnumerable<string> Features => _features.Keys.ToArray();

        public int Id
        {
            get => _item.Id;
            set => _item.Id = value;
        }

        public string Name
        {
            get => _item.Name;
            set => _item.Name = value;
        }

        private string _parentFeatureName;
        public string ParentFeatureName
        {
            get => _parentFeatureName;
            set
            {
                if (value is null || value == "(none)")
                {
                    throw new ArgumentNullException(nameof(value));
                }
                else if (_features.Any())
                {
                    var parent = _features[value];
                    _item.Feature = parent;
                    _item.FeatureId = parent.Id;
                }
                _parentFeatureName = value;
            }
        }

        public string Title { get => _item.Title; set => _item.Title = value; }
    }

    public class EditFeatureViewModel
    {
        private readonly Feature _feature;
        private readonly IDictionary<string, Feature> _topLevelFeatures;

        public EditFeatureViewModel() : this(new(), Enumerable.Empty<Feature>()) { }

        public EditFeatureViewModel(Feature feature, IEnumerable<Feature> features)
        {
            _feature = feature;
            _topLevelFeatures = features.Where(e => e.ParentId is null).ToDictionary(e => e.Name, e => e);

            ParentFeatureName = feature.ParentFeature?.Name;
        }

        public bool IsPersisted => _feature.Id != default;
        public IEnumerable<string> TopLevelFeatures => _topLevelFeatures.Keys.ToArray();

        public int Id
        {
            get => _feature.Id;
            set => _feature.Id = value;
        }

        public string Name
        {
            get => _feature.Name;
            set => _feature.Name = value;
        }

        private string _parentFeatureName;
        public string ParentFeatureName
        {
            get => _parentFeatureName;
            set
            {
                _parentFeatureName = value;
                if (value is null || value == "(none)")
                {
                    _feature.ParentFeature = null;
                    _feature.ParentId = null;
                }
                else if (_topLevelFeatures.Any())
                {
                    var parent = _topLevelFeatures[value];
                    _feature.ParentFeature = parent;
                    _feature.ParentId = parent.Id;
                }
            }
        }

        public string Title { get => _feature.Title; set => _feature.Title = value; }
        public string ElevatorPitch { get => _feature.ElevatorPitch; set => _feature.ElevatorPitch = value; }
        public string Description { get => _feature.Description; set => _feature.Description = value; }
        public string DescriptionAsHtml => new MarkdownSharp.Markdown().Transform(Description);

        public bool IsNew { get => _feature.IsNew; set => _feature.IsNew = value; }
        public bool IsHidden { get => _feature.IsHidden; set => _feature.IsHidden = value; }
        public int SortOrder { get => _feature.SortOrder; set => _feature.SortOrder = value; }

        public bool IsProtected => Feature.ProtectedFeatures.Contains(Name);

        public IEnumerable<FeatureItem> FeatureItems => _feature.FeatureItems;

        public Feature GetModel(IEnumerable<Feature> features)
        {
            if (_parentFeatureName != null && _parentFeatureName != "(none)")
            {
                var parent = features.SingleOrDefault(e => e.Name == _parentFeatureName);
                _feature.ParentFeature = parent;
                _feature.ParentId = parent?.Id;
            }
            return _feature;
        }
    }
}
