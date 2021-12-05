CREATE TABLE [site].[ExampleModules]
(
	[Id] INT IDENTITY (1,1) NOT NULL,
	[DateInserted] DATETIME NOT NULL,
	[DateUpdated] DATETIME NULL,
	[ExampleId] INT NOT NULL,
	[SortOrder] INT NOT NULL,
	[ModuleName] NVARCHAR(255) NOT NULL,
	[ModuleTypeId] INT NOT NULL,
	[Description] NVARCHAR(1024) NULL,
	[HtmlContent] NVARCHAR(MAX) NOT NULL
)
