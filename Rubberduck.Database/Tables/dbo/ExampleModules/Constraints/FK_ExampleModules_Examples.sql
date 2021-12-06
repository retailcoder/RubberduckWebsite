ALTER TABLE [dbo].[ExampleModules] ADD CONSTRAINT [FK_ExampleModules_Examples] FOREIGN KEY ([ExampleId]) REFERENCES [dbo].[Examples] ([Id]);
