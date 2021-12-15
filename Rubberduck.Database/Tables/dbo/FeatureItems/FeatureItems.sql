CREATE TABLE [dbo].[FeatureItems]
(
	[Id] INT IDENTITY (1,1) NOT NULL,
	[DateInserted] DATETIME NOT NULL,
	[DateUpdated] DATETIME NULL,
	[FeatureId] INT NOT NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Title] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1023) NOT NULL,
	[ContentUrl] NVARCHAR(1023) NULL,
	[IsNew] BIT NOT NULL,
	[IsDiscontinued] BIT NOT NULL,
	[IsHidden] BIT NOT NULL,
	[TagAssetId] INT NULL,
	[XmlDocSourceObject] NVARCHAR(1023) NULL,
	[XmlDocTabName] NVARCHAR(1023) NULL,
	[XmlDocMetadata] NVARCHAR(1023) NULL,
	[XmlDocSummary] NVARCHAR(MAX) NULL,
	[XmlDocInfo] NVARCHAR(MAX) NULL,
	[XmlDocRemarks] NVARCHAR(MAX) NULL
);
