CREATE TABLE [dbo].[TagAssets] 
(
	[Id] INT IDENTITY (1,1) NOT NULL,
	[DateInserted] DATETIME NOT NULL,
	[DateUpdated] DATETIME NULL,
	[TagId] INT NOT NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[DownloadUrl] NVARCHAR(1023) NOT NULL
)