﻿CREATE TABLE [dbo].[Features]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[DateInserted] DATETIME NOT NULL,
	[DateUpdated] DATETIME NULL,
	[ParentId] INT NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Title] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1023) NOT NULL,
	[ContentUrl] NVARCHAR(1023) NULL,
	[IsNew] BIT NOT NULL,
	[IsHidden] BIT NOT NULL,
	[SortOrder] INT NOT NULL,
	[XmlDocSource] NVARCHAR(1023) NULL
);
