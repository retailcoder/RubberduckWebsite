ALTER TABLE [site].[Examples] ADD CONSTRAINT [FK_Examples_FeatureItems] FOREIGN KEY ([FeatureItemId]) REFERENCES [site].[FeatureItems] ([Id]);
