using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rubberduck.API.Services.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Internal;

namespace Rubberduck.API.Services
{
    /// <summary>
    /// Regroups content writer service implementations.
    /// </summary>
    public class ContentServices : IContentServices
    {
        private readonly ILogger _logger;

        private readonly IContentReaderService<Feature> _featuresReader;
        private readonly IContentWriterService<Feature> _featuresWriter;
        private readonly IContentReaderService<FeatureItem> _featureItemsReader;
        private readonly IContentWriterService<FeatureItem> _featureItemsWriter;
        private readonly IContentReaderService<Example> _examplesReader;
        private readonly IContentWriterService<Example> _examplesWriter;
        private readonly IContentReaderService<ExampleModule> _exampleModulesReader;
        private readonly IContentWriterService<ExampleModule> _exampleModulesWriter;
        private readonly IContentReaderService<Tag> _tagsReader;
        private readonly IContentWriterService<Tag> _tagsWriter;
        private readonly IContentReaderService<TagAsset> _tagAssetsReader;
        private readonly IContentWriterService<TagAsset> _tagAssetsWriter;

        /// <summary>
        /// Creates a new service to orchestrate data access.
        /// </summary>
        public ContentServices(ILogger<ContentServices> logger,
            IContentReaderService<Feature> featuresReader,
            IContentWriterService<Feature> featuresWriter,
            IContentReaderService<FeatureItem> featureItemsReader,
            IContentWriterService<FeatureItem> featureItemsWriter,
            IContentReaderService<Example> examplesReader,
            IContentWriterService<Example> examplesWriter,
            IContentReaderService<ExampleModule> exampleModulesReader,
            IContentWriterService<ExampleModule> exampleModulesWriter,
            IContentReaderService<Tag> tagsReader,
            IContentWriterService<Tag> tagsWriter,
            IContentReaderService<TagAsset> tagAssetsReader,
            IContentWriterService<TagAsset> tagAssetsWriter)
        {
            _logger = logger;

            _featuresReader = featuresReader;
            _featuresWriter = featuresWriter;
            _featureItemsReader = featureItemsReader;
            _featureItemsWriter = featureItemsWriter;
            _examplesReader = examplesReader;
            _examplesWriter = examplesWriter;
            _exampleModulesReader = exampleModulesReader;
            _exampleModulesWriter = exampleModulesWriter;
            _tagsReader = tagsReader;
            _tagsWriter = tagsWriter;
            _tagAssetsReader = tagAssetsReader;
            _tagAssetsWriter = tagAssetsWriter;
        }

        /// <summary>
        /// Inserts or updates the specified data object.
        /// </summary>
        public async Task<Feature> SaveAsync(Model.DTO.Feature dto)
        {
            var entity = Feature.FromDTO(dto);
            if (entity.Id == 0)
            {
                _logger.LogDebug($"Creating new Feature entity...");
                return await _featuresWriter.CreateAsync(entity);
            }

            var existing = await _featuresReader.GetByIdAsync(entity.Id);
            if (existing is null)
            {
                _logger.LogWarning($"Could not find Feature Id {entity.Id}");
                return null;
            }
            else
            {
                _logger.LogDebug($"Updating existing Feature entity...");
                return await _featuresWriter.UpdateAsync(entity);
            }
        }

        /// <summary>
        /// Inserts or updates the specified data object.
        /// </summary>
        public async Task<FeatureItem> SaveAsync(Model.DTO.FeatureItem dto)
        {
            var entity = FeatureItem.FromDTO(dto);
            if (entity.Id == 0)
            {
                _logger.LogDebug($"Creating new FeatureItem entity...");
                return await _featureItemsWriter.CreateAsync(entity);
            }

            var existing = await _featureItemsReader.GetByIdAsync(entity.Id);
            if (existing is null)
            {
                _logger.LogWarning($"Could not find FeatureItem Id {entity.Id}");
                return null;
            }
            else
            {
                _logger.LogDebug($"Updating existing FeatureItem entity...");
                return await _featureItemsWriter.UpdateAsync(entity);
            }
        }

        /// <summary>
        /// Inserts or updates the specified data object.
        /// </summary>
        public async Task<Example> SaveAsync(Model.DTO.Example dto)
        {
            var entity = Example.FromDTO(dto);
            if (entity.Id == 0)
            {
                _logger.LogDebug($"Creating new Example entity...");
                return await _examplesWriter.CreateAsync(entity);
            }

            var existing = await _examplesReader.GetByIdAsync(entity.Id);
            if (existing is null)
            {
                _logger.LogWarning($"Could not find Example Id {entity.Id}");
                return null;
            }
            else
            {
                _logger.LogDebug($"Updating existing Example entity...");
                return await _examplesWriter.UpdateAsync(entity);
            }
        }

        /// <summary>
        /// Inserts or updates the specified data object.
        /// </summary>
        public async Task<ExampleModule> SaveAsync(Model.DTO.ExampleModule dto)
        {
            var entity = ExampleModule.FromDTO(dto);
            if (entity.Id == 0)
            {
                _logger.LogDebug($"Creating new ExampleModule entity...");
                return await _exampleModulesWriter.CreateAsync(entity);
            }

            var existing = await _exampleModulesReader.GetByIdAsync(entity.Id);
            if (existing is null)
            {
                _logger.LogWarning($"Could not find ExampleModule Id {entity.Id}");
                return null;
            }
            else
            {
                _logger.LogDebug($"Updating existing ExampleModule entity...");
                return await _exampleModulesWriter.UpdateAsync(entity);
            }
        }

        /// <summary>
        /// Inserts or updates the specified data object.
        /// </summary>
        public async Task<Tag> SaveAsync(Model.DTO.Tag dto)
        {
            var entity = Tag.FromDTO(dto);
            if (entity.Id == 0)
            {
                var existingName = await _tagsReader.GetByEntityKeyAsync(entity);
                if (existingName is null)
                {
                    _logger.LogDebug($"Creating new Tag {entity.Name}...");
                    return await _tagsWriter.CreateAsync(entity);
                }
            }
            else
            {
                var existingId = await _tagsReader.GetByIdAsync(entity.Id);
                if (existingId is null)
                {
                    _logger.LogWarning($"Could not find Tag Id {entity.Id}");
                    return null;
                }
            }

            _logger.LogDebug($"Updating existing Tag {entity.Name}...");
            return await _tagsWriter.UpdateAsync(entity);
        }

        /// <summary>
        /// Inserts or updates the specified data object.
        /// </summary>
        public async Task<TagAsset> SaveAsync(Model.DTO.TagAsset dto)
        {
            var entity = TagAsset.FromDTO(dto);
            if (entity.Id == 0)
            {
                var existingName = await _tagAssetsReader.GetByEntityKeyAsync(entity);
                if (existingName is null)
                {
                    _logger.LogDebug($"Creating new TagAsset entity...");
                    return await _tagAssetsWriter.CreateAsync(entity);
                }
            }
            else
            {
                var existingId = await _tagAssetsReader.GetByIdAsync(entity.Id);
                if (existingId is null)
                {
                    _logger.LogWarning($"Could not find TagAsset Id {entity.Id}");
                    return null;
                }
            }

            _logger.LogDebug($"Updating existing TagAsset entity...");
            return await _tagAssetsWriter.UpdateAsync(entity);
        }
    }
}
