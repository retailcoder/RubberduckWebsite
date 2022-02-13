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
            Feature = new FeatureViewModel(feature);
            HtmlDescription = new Markdown().Transform(feature.Description);
        }

        public FeatureViewModel Feature { get; }

        public string HtmlDescription { get; }
    }
}