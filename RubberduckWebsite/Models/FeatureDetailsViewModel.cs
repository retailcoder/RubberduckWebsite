using System.Collections.Generic;
using System.Linq;
using Rubberduck.Model.Entities;

namespace RubberduckWebsite.Models
{
    public class FeatureDetailsViewModel
    {
        public FeatureDetailsViewModel(Feature feature)
        {
            Feature = feature;
        }

        public Feature Feature { get; }
    }
}