﻿using Rubberduck.Model.Abstract;

namespace Rubberduck.Model.Entities
{
    public class ExampleModule : Entity
    {
        public static ExampleModule ParseError(string name) => new()
        {
            ModuleName = name,
            HtmlContent = "(error parsing code example from source xmldoc)"
        };

        public int ExampleId { get; set; }
        public int SortOrder { get; set; }
        public string ModuleName { get; set; }
        public ExampleModuleType ModuleType { get; set; }
        public string Description { get; set; }
        public string HtmlContent { get; set; }

        public virtual Example Example { get; set; }
    }
}
