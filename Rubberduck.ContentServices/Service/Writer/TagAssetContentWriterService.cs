using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Entity;

namespace Rubberduck.ContentServices.Writer
{
    public class TagAssetContentWriterService : ContentWriterService<TagAsset, Model.DTO.TagAsset>
    {
        private readonly IWriterDbContext _context;

        public TagAssetContentWriterService(IWriterDbContext context)
        {
            _context = context;
        }

        protected override IAsyncWriteRepository<Model.DTO.TagAsset> Repository => _context.TagAssetsRepository;
        protected override TagAsset GetEntity(Model.DTO.TagAsset dto) => TagAsset.FromDTO(dto);
        protected override Model.DTO.TagAsset GetDTO(TagAsset entity) => TagAsset.ToDTO(entity);
    }
}
