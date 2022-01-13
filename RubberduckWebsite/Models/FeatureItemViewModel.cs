using System;
using Rubberduck.Model.Entities;

namespace RubberduckWebsite.Models
{
    public class FeatureItemViewModel
    {
        public FeatureItemViewModel(FeatureItem item)
        {
            DateUpdated = item.DateUpdated ?? item.DateInserted;
            Name = item.Name;
            Title = item.Title;
            Description = item.Description;
            IsDiscontinued = item.IsDiscontinued;
            IsNew = item.IsNew;
            IsHidden = item.IsHidden;
            XmlDocTabName = item.XmlDocTabName;
            XmlDocSummary = item.XmlDocSummary;
            XmlDocRemarks = item.XmlDocRemarks;
            XmlDocInfo = item.XmlDocInfo;
            XmlDocMetadata = item.XmlDocMetadata;
            XmlDocSourceObject = item.XmlDocSourceObject;
        }

        public DateTime DateUpdated { get; }
        public string Name { get; }
        public string Title { get; }
        public string Description { get; }

        public bool IsDiscontinued { get; }
        public bool IsNew { get; }
        public bool IsHidden { get; }

        public string XmlDocTabName { get; }
        public string XmlDocSummary { get; }
        public string XmlDocInfo { get; }
        public string XmlDocRemarks { get; }
        public string XmlDocMetadata { get; }
        public string XmlDocSourceObject { get; }
    }
}