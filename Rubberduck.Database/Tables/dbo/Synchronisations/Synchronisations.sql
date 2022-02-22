CREATE TABLE [dbo].[Synchronisations]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[DateInserted] DATETIME NOT NULL,
	[DateUpdated] DATETIME NULL,
	[ApiVersion] NVARCHAR(255) NOT NULL,
	[RequestIP] NVARCHAR(32) NOT NULL,
	[UserAgent] NVARCHAR(1023) NOT NULL,
	[Message] NVARCHAR(1023) NOT NULL,
	[TimestampStart] DATETIME NOT NULL,
	[TimestampEnd] DATETIME NULL,
	-- [Processing:0,Success:1,Failed:2]
	[StatusCode] INT NOT NULL DEFAULT(0),
)
