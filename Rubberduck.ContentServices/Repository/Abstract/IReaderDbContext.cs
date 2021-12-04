using System;

namespace Rubberduck.ContentServices.Repository.Abstract
{
    public interface IReaderDbContext : IDisposable
    {
        IAsyncReadRepository<Model.DTO.Feature> FeaturesRepository { get; }
        IAsyncReadRepository<Model.DTO.FeatureItem> FeatureItemsRepository { get; }
        IAsyncReadRepository<Model.DTO.Example> ExamplesRepository { get; }
        IAsyncReadRepository<Model.DTO.ExampleModule> ExampleModulesRepository { get; }
        IAsyncReadRepository<Model.DTO.Tag> TagsRepository { get; }
        IAsyncReadRepository<Model.DTO.TagAsset> TagAssetsRepository { get; }
    }
}
