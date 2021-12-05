ALTER TABLE [site].[ExampleModules] ADD CONSTRAINT [FK_ExampleModules_ModuleTypes] FOREIGN KEY ([ModuleTypeId]) REFERENCES [site].[ExampleModuleTypes] ([Id]);
