ALTER TABLE [dbo].[FeatureItems] ADD CONSTRAINT [FK_FeatureItems_Features] FOREIGN KEY ([FeatureId]) REFERENCES [dbo].[Features] ([Id]);
