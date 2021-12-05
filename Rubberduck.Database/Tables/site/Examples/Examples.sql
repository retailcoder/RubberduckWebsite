CREATE TABLE [site].[Examples]
(
	[Id] INT IDENTITY (1,1) NOT NULL,
	[DateInserted] DATETIME NOT NULL,
	[DateUpdated] DATETIME NULL,
	[FeatureItemId] INT NOT NULL,
	[SortOrder] INT NOT NULL,
	[HtmlContent] NVARCHAR(MAX) NOT NULL
);