ALTER TABLE [site].[Features] ADD CONSTRAINT [FK_Features_Features] FOREIGN KEY ([ParentId]) REFERENCES [site].[Features] ([Id]);
