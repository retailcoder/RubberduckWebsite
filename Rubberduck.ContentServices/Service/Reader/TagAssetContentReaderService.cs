using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Entity;

namespace Rubberduck.ContentServices.Reader
{
    public class TagAssetContentReaderService : ContentReaderService<TagAsset, Model.DTO.TagAsset>
    {
        private readonly IReaderDbContext _context;

        public TagAssetContentReaderService(IReaderDbContext context)
        {
            _context = context;
        }

        protected override IAsyncReadRepository<Model.DTO.TagAsset> Repository => _context.TagAssetsRepository;

        protected override Model.DTO.TagAsset GetDTO(TagAsset entity) => TagAsset.ToDTO(entity);

        protected override TagAsset GetEntity(Model.DTO.TagAsset dto) => TagAsset.FromDTO(dto);
    }
}
