IF NOT EXISTS (SELECT * FROM [dbo].[Features])
BEGIN
	DECLARE @inspections NVARCHAR(50) = 'Inspections',
			@quickfixes NVARCHAR(50) = 'QuickFixes',
			@annotations NVARCHAR(50) = 'Annotations';

	DECLARE @ts DATETIME = GETDATE();
	INSERT INTO [dbo].[Features] ([DateInserted],[ParentId],[Name],[Title],[Description],[ContentUrl],[IsNew],[IsHidden],[SortOrder],[XmlDocSource])
	VALUES
		(@ts,null,'CodeInspections','Code Inspections', 'TODO', null, 0, 0, 1, null),
		(@ts,1,@inspections,'Inspections', 'TODO', null, 0, 0, 1, null),
		(@ts,1,@quickfixes,'QuickFixes', 'TODO', null, 0, 0, 1, null),
		(@ts,null,@annotations,'Annotations', 'TODO', null, 0, 0, 1, null),
		(@ts,null,'Refactorings','Refactorings', 'TODO', null, 0, 0, 1, null),
		(@ts,null,'UnitTesting','Unit Testing', 'TODO', null, 0, 0, 1, null),
		(@ts,null,'Navigation','Navigation Tools', 'TODO', null, 0, 0, 1, null)
	;
END
GO