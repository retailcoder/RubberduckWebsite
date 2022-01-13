using System.Collections.Generic;
using System.Linq;
using Rubberduck.Model.Entities;

namespace RubberduckWebsite.Models
{
    public class InspectionDetailsViewModel
    {
        public InspectionDetailsViewModel(FeatureItem item)
        {
            Name = item.Title;
            Summary = item.XmlDocSummary;
            InspectionType = item.XmlDocTabName;
            DefaultSeverity = item.XmlDocMetadata;
            Content = item.Description;
            Remarks = item.XmlDocRemarks;
            QuickFixes = item.XmlDocInfo.Split(",").ToHashSet();
        }

        public string Name { get; }
        public string Summary { get; }
        public string InspectionType { get; }
        public string DefaultSeverity { get; }
        public string Content { get; }
        public string Remarks { get; }
        public IReadOnlySet<string> QuickFixes { get; }
    }
}