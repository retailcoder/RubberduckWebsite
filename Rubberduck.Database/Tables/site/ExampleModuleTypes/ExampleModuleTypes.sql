CREATE TABLE [site].[ExampleModuleTypes]
(
	[Id] INT IDENTITY (1,1) NOT NULL,
	[DateInserted] DATETIME NOT NULL,
	[DateUpdated] DATETIME NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(255) NOT NULL,
	[IconClass] NVARCHAR(255) NOT NULL
)
