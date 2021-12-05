ALTER TABLE [dbo].[Examples] ADD CONSTRAINT [FK_Examples_FeatureItems] FOREIGN KEY ([FeatureItemId]) REFERENCES [dbo].[FeatureItems] ([Id]);
