using System;

namespace Rubberduck.ContentServices.Repository.Abstract
{
    public interface IWriterDbContext : IDisposable
    {
        IAsyncWriteRepository<Model.DTO.Feature> FeaturesRepository { get; }
        IAsyncWriteRepository<Model.DTO.FeatureItem> FeatureItemsRepository { get; }
        IAsyncWriteRepository<Model.DTO.Example> ExamplesRepository { get; }
        IAsyncWriteRepository<Model.DTO.ExampleModule> ExampleModulesRepository { get; }
        IAsyncWriteRepository<Model.DTO.Tag> TagsRepository { get; }
        IAsyncWriteRepository<Model.DTO.TagAsset> TagAssetsRepository { get; }
    }
}
