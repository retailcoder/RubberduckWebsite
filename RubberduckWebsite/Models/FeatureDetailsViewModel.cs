using System.Collections.Generic;
using System.Linq;
using MarkdownSharp;
using Rubberduck.Model.Entities;

namespace RubberduckWebsite.Models
{
    public class FeatureDetailsViewModel
    {
        public FeatureDetailsViewModel(Feature feature)
        {
            Feature = feature;
            Description = new Markdown().Transform(feature.Description);
        }

        public Feature Feature { get; }

        public string Description { get; }
    }
}