using System;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Entity;

namespace Rubberduck.ContentServices.Writer
{
    public class FeatureItemContentWriterService : ContentWriterService<FeatureItem, Model.DTO.FeatureItem>
    {
        private readonly IWriterDbContext _context;

        public FeatureItemContentWriterService(IWriterDbContext context)
        {
            _context = context;
        }

        protected override IAsyncWriteRepository<Model.DTO.FeatureItem> Repository => _context.FeatureItemsRepository;
        protected override FeatureItem GetEntity(Model.DTO.FeatureItem dto) => FeatureItem.FromDTO(dto);
        protected override Model.DTO.FeatureItem GetDTO(FeatureItem entity) => FeatureItem.ToDTO(entity);
    }
}
