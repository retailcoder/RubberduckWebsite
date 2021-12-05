ALTER TABLE [site].[ExampleModules] ADD CONSTRAINT [FK_ExampleModules_Examples] FOREIGN KEY ([ExampleId]) REFERENCES [site].[Examples] ([Id]);
