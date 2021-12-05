IF NOT EXISTS (SELECT * FROM [dbo].[ExampleModuleTypes])
BEGIN
	DECLARE @ts DATETIME = GETDATE();
	INSERT INTO [dbo].[ExampleModuleTypes] ([DateInserted],[Name],[Description],[IconClass])
	VALUES
		(@ts,'(Any)','Any type of module', 'moduletype-any'),
		(@ts,'Class Module','Any class module','moduletype-cls'),
		(@ts,'Document Module','A document class module','moduletype-doc'),
		(@ts,'Interface Module','A class module defining an abstract interface','moduletype-abs'),
		(@ts,'Predeclared Class','Any class with a VB_PredeclaredId attribute value of "True"','moduletype-pid'),
		(@ts,'Standard Module','A standard procedural module','moduletype-bas'),
		(@ts,'UserForm Module','A UserForm class module','moduletype-frm')
END
