ALTER TABLE [site].[FeatureItems] ADD CONSTRAINT [FK_FeatureItems_Features] FOREIGN KEY ([FeatureId]) REFERENCES [site].[Features] ([Id]);
