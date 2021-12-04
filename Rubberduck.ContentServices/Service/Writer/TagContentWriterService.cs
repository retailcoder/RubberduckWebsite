using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Entity;

namespace Rubberduck.ContentServices.Writer
{
    public class TagContentWriterService : ContentWriterService<Tag, Model.DTO.Tag>
    {
        private readonly IWriterDbContext _context;

        public TagContentWriterService(IWriterDbContext context)
        {
            _context = context;
        }

        protected override IAsyncWriteRepository<Model.DTO.Tag> Repository => _context.TagsRepository;
        protected override Tag GetEntity(Model.DTO.Tag dto) => Tag.FromDTO(dto);
        protected override Model.DTO.Tag GetDTO(Tag entity) => Tag.ToDTO(entity);
    }
}
