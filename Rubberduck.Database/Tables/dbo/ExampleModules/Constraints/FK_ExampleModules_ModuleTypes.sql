ALTER TABLE [dbo].[ExampleModules] ADD CONSTRAINT [FK_ExampleModules_ModuleTypes] FOREIGN KEY ([ModuleTypeId]) REFERENCES [dbo].[ExampleModuleTypes] ([Id]);
