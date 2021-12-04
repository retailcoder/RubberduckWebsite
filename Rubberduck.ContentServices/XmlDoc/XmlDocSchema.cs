namespace Rubberduck.ContentServices.XmlDoc
{
    public static class XmlDocSchema
    {
        public static class Inspection
        {
            public static class Config
            {
                public static readonly string InspectionTypeAttribute = "InspectionType";
                public static readonly string SeverityAttribute = "Severity";
            }

            public static class Summary
            {
                public static readonly string ElementName = "summary";
                public static readonly string IsHiddenAttribute = "hidden";
            }

            public static class Reference
            {
                public static readonly string ElementName = "reference";
                public static readonly string NameAttribute = "name";
            }

            public static class HostApp
            {
                public static readonly string ElementName = "hostapp";
                public static readonly string NameAttribute = "name";
            }

            public static class Reasoning
            {
                public static readonly string ElementName = "why";
            }

            public static class Remarks
            {
                public static readonly string ElementName = "remarks";
            }

            public static class Examples
            {
                public static readonly string ElementName = "example";
                public static readonly string HasResultAttribute = "hasresult";

                public static class Modules
                {
                    public static readonly string ElementName = "module";
                    public static readonly string ModuleNameAttribute = "name";
                    public static readonly string ModuleTypeAttribute = "type";
                }
            }
        }
    }
}
