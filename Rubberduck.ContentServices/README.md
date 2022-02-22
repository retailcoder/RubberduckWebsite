# Rubberduck.ContentServices  
Contains the logic for accessing the backend database and parsing xmldocs into website content.

Content services are exposed via the **IContentService** interface.

**IXmlDocParser** exposes methods to parse Rubberduck's xmldoc into `FeatureItem` objects.

Consider the `ContentServices.Model` entity types as though they were `internal`, and expose `Rubberduck.Model.Entities` types instead.