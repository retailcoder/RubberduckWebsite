using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rubberduck.Model.Entities;

namespace RubberduckWebsite.Models
{
    public class EditFeatureViewModel
    {
        private readonly Feature _feature;
        private readonly IDictionary<string, Feature> _topLevelFeatures;

        public EditFeatureViewModel() : this(new(), Enumerable.Empty<Feature>()) { }

        public EditFeatureViewModel(Feature feature, IEnumerable<Feature> features)
        {
            _feature = feature;
            _topLevelFeatures = features.Where(e => e.ParentId is null).ToDictionary(e => e.Name, e => e);
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
                else if(_topLevelFeatures.Any())
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


        public Feature GetModel() => _feature;
    }
}
