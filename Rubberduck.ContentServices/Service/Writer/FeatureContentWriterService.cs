using System;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Entity;

namespace Rubberduck.ContentServices.Writer
{
    public class FeatureContentWriterService : ContentWriterService<Feature, Model.DTO.Feature>
    {
        private readonly IWriterDbContext _context;

        public FeatureContentWriterService(IWriterDbContext context)
        {
            _context = context;
        }

        protected override IAsyncWriteRepository<Model.DTO.Feature> Repository => _context.FeaturesRepository;
        protected override Feature GetEntity(Model.DTO.Feature dto) => Feature.FromDTO(dto);
        protected override Model.DTO.Feature GetDTO(Feature entity) => Feature.ToDTO(entity);
    }
}
