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
            dto.DateInserted = DateTime.UtcNow;

            await _context.FeatureItems.AddAsync(dto);
            await _context.SaveChangesAsync();
            return FeatureItem.FromDTO(dto);
        }

        public async Task<FeatureItem> UpdateAsync(FeatureItem entity) => await Task.Run(() =>
        {
            var dto = _context.FeatureItems.AsTracking().SingleOrDefault(e => e.Id == entity.Id || (e.FeatureId == entity.FeatureId && e.Name == entity.Name));
            if (IsDirty(entity, dto))
            {
                dto.DateUpdated = DateTime.UtcNow;
                dto.ContentUrl = entity.ContentUrl?.ToString();
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
            }
            return FeatureItem.FromDTO(dto);
        });
        private static bool IsDirty(FeatureItem model, Model.DTO.FeatureItemEntity dto) =>
            model.ContentUrl?.ToString() != dto.ContentUrl
            || model.Description != dto.Description 
            || model.IsHidden != dto.IsHidden
            || model.IsNew != dto.IsNew
            || model.TagAssetId != dto.TagAssetId
            || model.Title != dto.Title
            || model.Info != dto.XmlDocInfo
            || model.Metadata != dto.XmlDocMetadata
            || model.Remarks != dto.XmlDocRemarks
            || model.RubberduckSource != dto.XmlDocSourceObject
            || model.Summary != dto.XmlDocSummary
            || model.TabName != dto.XmlDocTabName;

        public async Task DeleteAsync(FeatureItem entity) => await Task.Run(() =>
        {
            var dto = _context.FeatureItems.AsTracking().SingleOrDefault(e => e.Id == entity.Id);
            _context.Remove(dto);
        });
    }
}
