ALTER TABLE [dbo].[FeatureItems] ADD CONSTRAINT [FK_FeatureItems_TagAssets] FOREIGN KEY ([TagAssetId]) REFERENCES [dbo].[TagAssets] ([Id]);
