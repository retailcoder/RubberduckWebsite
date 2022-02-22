using System;
using System.Text.RegularExpressions;
using Rubberduck.Model.Entities;

namespace RubberduckWebsite.Models
{
    public class FeatureItemViewModel
    {
        public FeatureItemViewModel(FeatureItem entity)
        {
            DateUpdated = entity.DateUpdated ?? entity.DateInserted;
            Name = entity.Name;
            Title = SplitPascalCase(entity.Title);
            Description = entity.Description;
            IsDiscontinued = entity.IsDiscontinued;
            IsNew = entity.IsNew;
            IsHidden = entity.IsHidden;
            XmlDocTabName = entity.XmlDocTabName;
            XmlDocSummary = entity.XmlDocSummary;
            XmlDocRemarks = entity.XmlDocRemarks;
            XmlDocInfo = entity.XmlDocInfo;
            XmlDocMetadata = entity.XmlDocMetadata;
            XmlDocSourceObject = entity.XmlDocSourceObject;
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

        private static string SplitPascalCase(string value)
        {
            const string pattern = "(?<=[A-Za-z])(?=[A-Z][a-z])|(?<=[a-z0-9])(?=[0-9]?[A-Z])";
            var nocaps = new[] { " a ", " an ", " of ", " the ", " in ", " at ", " on ", " is ", " to " };

            var result = value.StartsWith("@")
                ? value
                : Regex.Replace(value, pattern, " ")
                    .Replace("Hot Key", "Hotkey")
                    .Replace("By Val", "ByVal")
                    .Replace("By Ref", "ByRef")
                    .Replace("I If", "IIf")
                    .Replace("Def Type", "DefType");

            foreach (var word in nocaps)
            {
                result = result.Replace(word, word.ToLowerInvariant(), StringComparison.InvariantCultureIgnoreCase);
            }

            return result;
        }
    }
}