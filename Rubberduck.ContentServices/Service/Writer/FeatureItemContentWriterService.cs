using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Internal;

namespace Rubberduck.ContentServices.Writer
{
    public class FeatureItemContentWriterService : IContentWriterService<FeatureItem>
    {
        private readonly RubberduckDbContext _context;

        public FeatureItemContentWriterService(RubberduckDbContext context)
        {
            _context = context;
        }

        public async Task<FeatureItem> CreateAsync(FeatureItem entity)
        {
            if (entity.Id != default)
            {
                throw new InvalidOperationException("Cannot add an entity that already has an ID.");
            }

            var dto = FeatureItem.ToDTO(entity);
            dto.DateInserted = DateTime.Now;

            await _context.FeatureItems.AddAsync(dto);

            await _context.SaveChangesAsync();
            return FeatureItem.FromDTO(dto);
        }

        public async Task<FeatureItem> UpdateAsync(FeatureItem entity)
        {
            var dto = _context.FeatureItems.AsTracking().SingleOrDefault(e => e.Id == entity.Id);
            dto.DateUpdated = DateTime.Now;
            dto.ContentUrl = entity.ContentUrl.ToString();
            dto.Description = entity.Description;
            dto.IsHidden = entity.IsHidden;
            dto.IsNew = entity.IsNew;
            dto.Name = entity.Name;
            dto.TagAssetId = entity.TagAssetId;
            dto.Title = entity.Title;
            dto.XmlDocInfo = entity.Info;
            dto.XmlDocMetadata = entity.Metadata;
            dto.XmlDocRemarks = entity.Remarks;
            dto.XmlDocSourceObject = entity.RubberduckSource;
            dto.XmlDocSummary = entity.Summary;
            dto.XmlDocTabName = entity.TabName;
            
            await _context.SaveChangesAsync();
            return FeatureItem.FromDTO(dto);
        }

        public async Task DeleteAsync(FeatureItem entity)
        {
            var dto = _context.FeatureItems.AsTracking().SingleOrDefault(e => e.Id == entity.Id);
            _context.Remove(dto);

            await _context.SaveChangesAsync();
        }
    }
}
