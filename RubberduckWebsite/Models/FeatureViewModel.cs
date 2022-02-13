using System;
using System.Collections.Generic;
using System.Linq;
using Rubberduck.Model.Entities;

namespace RubberduckWebsite.Models
{
    public class FeatureViewModel
    {
        public FeatureViewModel(Feature entity)
        {
            DateUpdated = entity.DateUpdated ?? entity.DateInserted;
            Name = entity.Name;
            ElevatorPitch = entity.ElevatorPitch;
            Title = entity.Title;
            Description = entity.Description;
            IsNew = entity.IsNew;
            IsHidden = entity.IsHidden;
            SortOrder = entity.SortOrder;
            SubFeatures = entity.SubFeatures.Select(e => new FeatureViewModel(e));
            FeatureItems = entity.FeatureItems.Select(e => new FeatureItemViewModel(e));
        }

        public DateTime DateUpdated { get; }
        public string Name { get; }
        public string Title { get; }
        public string ElevatorPitch { get; }
        public string Description { get; }

        public bool IsDiscontinued { get; }
        public bool IsNew { get; }
        public bool IsHidden { get; }

        public int SortOrder { get; }

        public IEnumerable<FeatureViewModel> SubFeatures { get; }

        public IEnumerable<FeatureItemViewModel> FeatureItems { get; }
    }
}