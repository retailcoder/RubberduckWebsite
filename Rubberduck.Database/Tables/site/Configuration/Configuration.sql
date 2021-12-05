﻿CREATE TABLE [site].[Configuration]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[DateInserted] DATETIME NOT NULL,
	[DateUpdated] DATETIME NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Value] NVARCHAR(MAX) NOT NULL
)
