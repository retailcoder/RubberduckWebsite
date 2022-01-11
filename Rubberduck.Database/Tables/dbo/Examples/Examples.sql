CREATE TABLE [dbo].[Examples]
(
	[Id] INT IDENTITY (1,1) NOT NULL,
	[DateInserted] DATETIME NOT NULL,
	[DateUpdated] DATETIME NULL,
	[FeatureItemId] INT NOT NULL,
	[SortOrder] INT NOT NULL,
	[Description] NVARCHAR(1023) NOT NULL
);