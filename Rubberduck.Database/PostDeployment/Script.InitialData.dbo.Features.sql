IF NOT EXISTS (SELECT * FROM [dbo].[Features])
BEGIN
	DECLARE @inspections NVARCHAR(50) = 'Inspections',
			@quickfixes NVARCHAR(50) = 'QuickFixes',
			@annotations NVARCHAR(50) = 'Annotations';

	DECLARE @ts DATETIME = GETDATE();
	INSERT INTO [dbo].[Features] ([DateInserted],[ParentId],[Name],[Title],[ElevatorPitch],[Description],[IsNew],[IsHidden],[SortOrder],[XmlDocSource])
	VALUES
		(@ts,null,'CodeInspections','Code Inspections', 'A short (max. 1023 chars) description of the feature.', 'A detailed description of the feature. Markdown is expected.', 0, 0, 1, null),
		(@ts,1,@inspections,'Inspections', 'A short description of the feature.', 'A detailed description of the feature. Markdown is expected.', 0, 0, 1, null),
		(@ts,1,@quickfixes,'QuickFixes', 'A short description of the feature.', 'A detailed description of the feature. Markdown is expected.', 0, 0, 1, null),
		(@ts,null,'CommentAnnotations','Annotations', 'A short description of the feature.', 'A detailed description of the feature. Markdown is expected.', 0, 0, 1, null),
		(@ts,4,@annotations,'Annotations', 'A short description of the feature.', 'A detailed description of the feature. Markdown is expected.', 0, 0, 1, null),

		(@ts,null,'Refactorings','Refactorings', 'A short description of the feature.', 'A detailed description of the feature. Markdown is expected.', 0, 0, 1, null),
		(@ts,null,'UnitTesting','Unit Testing', 'A short description of the feature.', 'A detailed description of the feature. Markdown is expected.', 0, 0, 1, null),
		(@ts,null,'Navigation','Navigation Tools', 'A short description of the feature.', 'A detailed description of the feature. Markdown is expected.', 0, 0, 1, null)
	;
END
GO