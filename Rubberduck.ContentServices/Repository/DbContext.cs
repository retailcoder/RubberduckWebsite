using System;
using System.Data;
using System.Data.SqlClient;
using Rubberduck.ContentServices.Repository;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.Model.DTO;

namespace Rubberduck.ContentServices
{
    public sealed class DbContext : IReaderDbContext, IWriterDbContext, IDisposable
    {
        private readonly IDbConnection _connection;
        private readonly FeaturesRepository _featuresRepository;
        private readonly FeatureItemsRepository _featureItemsRepository;
        private readonly ExamplesRepository _examplesRepository;
        private readonly ExampleModulesRepository _exampleModulesRepository;
        private readonly TagsRepository _tagsRepository;
        private readonly TagAssetsRepository _tagAssetsRepository;

        public DbContext(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _featuresRepository = new(_connection);
            _featureItemsRepository = new(_connection);
            _examplesRepository = new(_connection);
            _exampleModulesRepository = new(_connection);
            _tagsRepository = new(_connection);
            _tagAssetsRepository = new(_connection);
        }

        IAsyncReadRepository<Feature> IReaderDbContext.FeaturesRepository => _featuresRepository;
        IAsyncReadRepository<FeatureItem> IReaderDbContext.FeatureItemsRepository => _featureItemsRepository;
        IAsyncReadRepository<Example> IReaderDbContext.ExamplesRepository => _examplesRepository;
        IAsyncReadRepository<ExampleModule> IReaderDbContext.ExampleModulesRepository => _exampleModulesRepository;
        IAsyncReadRepository<Tag> IReaderDbContext.TagsRepository => _tagsRepository;
        IAsyncReadRepository<TagAsset> IReaderDbContext.TagAssetsRepository => _tagAssetsRepository;


        IAsyncWriteRepository<Feature> IWriterDbContext.FeaturesRepository => _featuresRepository;
        IAsyncWriteRepository<FeatureItem> IWriterDbContext.FeatureItemsRepository => _featureItemsRepository;
        IAsyncWriteRepository<Example> IWriterDbContext.ExamplesRepository => _examplesRepository;
        IAsyncWriteRepository<ExampleModule> IWriterDbContext.ExampleModulesRepository => _exampleModulesRepository;
        IAsyncWriteRepository<Tag> IWriterDbContext.TagsRepository => _tagsRepository;
        IAsyncWriteRepository<TagAsset> IWriterDbContext.TagAssetsRepository => _tagAssetsRepository;

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
