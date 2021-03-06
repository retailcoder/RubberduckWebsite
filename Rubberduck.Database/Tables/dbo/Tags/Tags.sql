CREATE TABLE [dbo].[Tags]
(
	[Id] INT IDENTITY (1,1) NOT NULL,
	[DateInserted] DATETIME NOT NULL,
	[DateUpdated] DATETIME NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[InstallerDownloadUrl] NVARCHAR(1023) NULL,
	[InstallerDownloads] INT NOT NULL,
	[IsPreRelease] BIT NOT NULL
)
