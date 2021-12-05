using System.Threading.Tasks;
using Rubberduck.API.Services.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Entity;

namespace Rubberduck.API.Services
{
    public class ContentServices : IContentServices
    {
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

        public ContentServices(
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

        public async Task<Feature> SaveAsync(Model.DTO.Feature dto)
        {
            var entity = Feature.FromDTO(dto);
            if (entity.Id == 0)
            {
                return await _featuresWriter.CreateAsync(entity);
            }

            var existing = await _featuresReader.GetByIdAsync(entity.Id);
            if (existing is null)
            {
                return null;
            }
            else
            {
                return await _featuresWriter.UpdateAsync(entity);
            }
        }

        public async Task<FeatureItem> SaveAsync(Model.DTO.FeatureItem dto)
        {
            var entity = FeatureItem.FromDTO(dto);
            if (entity.Id == 0)
            {
                return await _featureItemsWriter.CreateAsync(entity);
            }

            var existing = await _featureItemsReader.GetByIdAsync(entity.Id);
            if (existing is null)
            {
                return null;
            }
            else
            {
                return await _featureItemsWriter.UpdateAsync(entity);
            }
        }

        public async Task<Example> SaveAsync(Model.DTO.Example dto)
        {
            var entity = Example.FromDTO(dto);
            if (entity.Id == 0)
            {
                return await _examplesWriter.CreateAsync(entity);
            }

            var existing = await _examplesReader.GetByIdAsync(entity.Id);
            if (existing is null)
            {
                return null;
            }
            else
            {
                return await _examplesWriter.UpdateAsync(entity);
            }
        }

        public async Task<ExampleModule> SaveAsync(Model.DTO.ExampleModule dto)
        {
            var entity = ExampleModule.FromDTO(dto);
            if (entity.Id == 0)
            {
                return await _exampleModulesWriter.CreateAsync(entity);
            }

            var existing = await _exampleModulesReader.GetByIdAsync(entity.Id);
            if (existing is null)
            {
                return null;
            }
            else
            {
                return await _exampleModulesWriter.UpdateAsync(entity);
            }
        }

        public async Task<Tag> SaveAsync(Model.DTO.Tag dto)
        {
            var entity = Tag.FromDTO(dto);
            if (entity.Id == 0)
            {
                return await _tagsWriter.CreateAsync(entity);
            }

            var existing = await _tagsReader.GetByIdAsync(entity.Id);
            if (existing is null)
            {
                return null;
            }
            else
            {
                return await _tagsWriter.UpdateAsync(entity);
            }
        }

        public async Task<TagAsset> SaveAsync(Model.DTO.TagAsset dto)
        {
            var entity = TagAsset.FromDTO(dto);
            if (entity.Id == 0)
            {
                return await _tagAssetsWriter.CreateAsync(entity);
            }

            var existing = await _tagAssetsReader.GetByIdAsync(entity.Id);
            if (existing is null)
            {
                return null;
            }
            else
            {
                return await _tagAssetsWriter.UpdateAsync(entity);
            }
        }
    }
}
