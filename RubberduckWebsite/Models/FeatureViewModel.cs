using System;
using System.Collections.Generic;
using System.IO;
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
            IsProtected = entity.IsProtected;
            SortOrder = entity.SortOrder;
            SubFeatures = entity.SubFeatures.Select(e => new FeatureViewModel(e));
            FeatureItems = entity.FeatureItems.Select(e => new FeatureItemViewModel(e));

            var source = FindScreenshot();
            if (source != null)
            {
                HasScreenshot = true;
                ScreenshotSource = source;
            }
        }

        private string FindScreenshot()
        {
            var extensions = new[] { "gif", "png" };
            foreach (var ext in extensions)
            {
                var source = $"wwwroot/images/features/{Name}.{ext}";
                if (File.Exists(source))
                {
                    return $"images/features/{Name}.{ext}";
                }
            }

            return null;
        }

        public DateTime DateUpdated { get; }
        public string Name { get; }
        public string Title { get; }
        public string ElevatorPitch { get; }
        public string Description { get; }

        public bool IsDiscontinued { get; }
        public bool IsNew { get; }
        public bool IsProtected { get; }
        public bool IsHidden { get; }

        public bool HasScreenshot { get; }
        public string ScreenshotSource { get; }

        public int SortOrder { get; }

        public IEnumerable<FeatureViewModel> SubFeatures { get; }

        public IEnumerable<FeatureItemViewModel> FeatureItems { get; }
    }
}