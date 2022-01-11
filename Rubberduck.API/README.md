# Rubberduck.API  
A .NET Web API that serves everything the website needs, publishes to https://api.rubberduckvba.com.

## Public API  
The `PublicController` exposes the following endpoints:

### [GET] GetFeaturesAsync()  
Gets all top-level features along with their respective sub-features and feature items.

### [GET] GetFeatureAsync(string)  
Gets a features or sub-features, with its feature items.

### [GET] GetFeatureItemAsync(int)  
Gets the specified feature item, including its examples and their respective modules.

### [GET] GetLatestTagsAsync()  
Gets the latest release and prerelease tags.

### [GET] GetTagAssets(int)  
Gets the xmldoc assets for the specified tag.

### [POST] IndentAsync(IndenterViewModel)  
Gets the supplied code, indented as per specified settings.
