IF NOT EXISTS (SELECT * FROM [site].[Configuration])
BEGIN
	DECLARE @ts DATETIME = GETDATE();
	INSERT INTO [site].[Configuration] ([DateInserted],[Name],[Value])
	VALUES
		(@ts,'XmlDocInfo_HostApp','<p id=\"host_or_library_specific_info\"><span class=\"icon icon-info\"></span>This inspection will only run if the host application is <code>{0}</code>.</p>'),
		(@ts,'XmlDocInfo_SingleLibrary', '<p id=\"host_or_library_specific_info\"><span class=\"icon icon-info\"></span>This inspection will only run if the <code>{0}</code> library is referenced.</p>'),
		(@ts,'XmlDocInfo_MultipleLibraries', '<p id=\"host_or_library_specific_info\"><span class=\"icon icon-info\"></span>This inspection will only run if one or a combination of the following libraries is referenced: {libraries}</p>')

END
