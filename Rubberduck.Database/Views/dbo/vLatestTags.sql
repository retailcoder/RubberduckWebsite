CREATE VIEW [dbo].[vLatestTags] AS 

	WITH prereleases AS (
		SELECT t.[Id], [Filter] = ROW_NUMBER() OVER (ORDER BY t.[DateCreated] DESC)
		FROM [dbo].[Tags] t
		WHERE t.[IsPreRelease] = 1
	), releases AS (
		SELECT t.[Id], [Filter] = ROW_NUMBER() OVER (ORDER BY t.[DateCreated] DESC)
		FROM [dbo].[Tags] t
		WHERE t.[IsPreRelease] = 0
	), latest AS (
		SELECT r.[Id] FROM releases r WHERE r.[Filter] = 1
		UNION ALL
		SELECT r.[Id] FROM prereleases r WHERE r.[Filter] = 1
	)
	SELECT
		t.[Id],
		t.[DateInserted],
		t.[DateUpdated],
		t.[DateCreated],
		t.[Name],
		t.[InstallerDownloads],
		t.[InstallerDownloadUrl],
		t.[IsPreRelease]
	FROM [dbo].[Tags] t
	INNER JOIN latest ON t.[Id] = latest.[Id]
