ALTER TABLE [dbo].[Features] ADD CONSTRAINT [FK_Features_Features] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Features] ([Id]);
