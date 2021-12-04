using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Entity;

namespace Rubberduck.ContentServices.Reader
{
    public class ReleaseTagContentReaderService : ContentReaderService<Tag, Model.DTO.Tag>
    {
        private readonly IReaderDbContext _context;

        public ReleaseTagContentReaderService(IReaderDbContext context)
        {
            _context = context;
        }

        private async Task<IEnumerable<Tag>> GetLatestTags() => 
            await await ((Repository.TagsRepository)Repository).GetLatestTagsAsync().ContinueWith(async t => await GetTagsAsync(t.Result));

        protected override IAsyncReadRepository<Model.DTO.Tag> Repository => _context.TagsRepository;

        protected override Model.DTO.Tag GetDTO(Tag entity) => Tag.ToDTO(entity);

        protected override Tag GetEntity(Model.DTO.Tag dto) => Tag.FromDTO(dto);

        public override async Task<IEnumerable<Tag>> GetAllAsync() => await GetLatestTags();
            //await await Repository.GetAllAsync().ContinueWith(async t => await GetTagsAsync(t.Result));

        public override async Task<Tag> GetByIdAsync(int id) =>
            await await Repository.GetByIdAsync(id).ContinueWith(async t => await GetTagAsync(t.Result));

        public override async Task<Tag> GetByEntityKeyAsync(object key) =>
            await await Repository.GetByKeyAsync(key).ContinueWith(async t => await GetTagAsync(t.Result));

        private async Task<IEnumerable<Tag>> GetTagsAsync(IEnumerable<Model.DTO.Tag> tags)
        {
            var results = new List<Tag>();
            foreach (var tag in tags)
            {
                results.Add(await GetTagAsync(tag));
            }
            return results;
        }

        private async Task<Tag> GetTagAsync(Model.DTO.Tag dto)
        {
            if (dto is null)
            {
                return null;
            }
            var assets = await GetTagAssetsAsync(dto);
            return Tag.FromDTO(dto, assets);
        }

        private async Task<IEnumerable<TagAsset>> GetTagAssetsAsync(Model.DTO.Tag dto)
        {
            if (dto is null)
            {
                return Enumerable.Empty<TagAsset>();
            }
            return await _context.TagAssetsRepository.GetAllAsync(dto.Id)
                .ContinueWith(t => t.Result.Select(TagAsset.FromDTO));
        }
    }
}
