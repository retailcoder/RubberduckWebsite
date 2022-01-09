using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Entities;

namespace Rubberduck.ContentServices.Service
{
    public class ContentService : IContentService
    {
        private readonly RubberduckDbContext _context;

        public ContentService(RubberduckDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Feature>> GetFeaturesAsync()
        {
            var entities = await _context.Features
                .Include(entity => entity.SubFeatures)
                .Include(entity => entity.FeatureItems)
                .Where(entity => entity.ParentId == null)
                .ToListAsync();

            return entities.Select(e => e.ToPublicModel());
        }

        public async Task<Feature> GetFeatureAsync(string name)
        {
            var result = await _context.Features
                .Where(entity => entity.Name == name)
                .Include(entity => entity.SubFeatures)
                .Include(entity => entity.FeatureItems)
                .SingleOrDefaultAsync();

            return result?.ToPublicModel();
        }

        public async Task<FeatureItem> GetFeatureItemAsync(string name)
        {
            var result = await _context.FeatureItems
                .Where(entity => entity.Name == name)
                .Include(entity => entity.Feature)
                .Include(entity => entity.TagAsset)
                .Include(entity => entity.Examples).ThenInclude(example => example.Modules)
                .SingleOrDefaultAsync();

            return result?.ToPublicModel();
        }

        public async Task<Tag> GetMainTagAsync()
        {
            var entity = await _context.Tags
                .Where(entity => !entity.IsPreRelease)
                .OrderByDescending(entity => entity.DateCreated)
                .Include(entity => entity.TagAssets)
                .FirstOrDefaultAsync();

            return entity.ToPublicModel();
        }

        public async Task<Tag> GetNextTagAsync()
        {
            var entity = await _context.Tags
                .Where(entity => entity.IsPreRelease)
                .OrderByDescending(entity => entity.DateCreated)
                .Include(entity => entity.TagAssets)
                .FirstOrDefaultAsync();

            return entity.ToPublicModel();
        }

        public async Task<Example> SaveExampleAsync(Example model)
        {
            if (model.Id == default)
            {
                var entity = new Model.Example(model)
                {
                    DateInserted = DateTime.UtcNow
                };

                await _context.Examples.AddAsync(entity);
                return entity.ToPublicModel();
            }

            var existing = await _context.Examples.AsTracking()
                .Include(entity => entity.Modules)
                .SingleOrDefaultAsync(entity => entity.Id == model.Id);

            existing.DateUpdated = DateTime.UtcNow;
            existing.Description = model.Description;
            existing.SortOrder = model.SortOrder;

            await _context.SaveChangesAsync();
            return existing.ToPublicModel();
        }

        public async Task<ExampleModule> SaveExampleModuleAsync(ExampleModule model)
        {
            if (model.Id == default)
            {
                var entity = new Model.ExampleModule(model)
                {
                    DateInserted = DateTime.UtcNow
                };

                await _context.ExampleModules.AddAsync(entity);
                return entity.ToPublicModel();
            }

            var existing = await _context.ExampleModules.AsTracking()
                .SingleOrDefaultAsync(entity => entity.Id == model.Id);

            existing.DateUpdated = DateTime.UtcNow;
            existing.Description = model.Description;
            existing.HtmlContent = model.HtmlContent;
            existing.ModuleName = model.ModuleName;
            existing.ModuleTypeId = (int)model.ModuleType;
            existing.SortOrder = model.SortOrder;

            await _context.SaveChangesAsync();
            return existing.ToPublicModel();
        }

        public async Task<Feature> SaveFeatureAsync(Feature model)
        {
            if (model.Id == default)
            {
                var entity = new Model.Feature(model)
                {
                    DateInserted = DateTime.UtcNow
                };

                await _context.Features.AddAsync(entity);
                return entity.ToPublicModel();
            }

            var existing = await _context.Features.AsTracking()
                .Include(entity => entity.FeatureItems)
                .Include(entity => entity.SubFeatures)
                .SingleOrDefaultAsync(entity => entity.Id == model.Id || entity.Name == model.Name);

            existing.DateUpdated = DateTime.UtcNow;
            existing.Description = model.Description;
            existing.ElevatorPitch = model.ElevatorPitch;
            existing.IsHidden = model.IsHidden;
            existing.IsNew = model.IsNew;
            existing.SortOrder = model.SortOrder;
            existing.Title = model.Title;
            existing.XmlDocSource = model.XmlDocSource;

            await _context.SaveChangesAsync();
            return existing.ToPublicModel();
        }

        public async Task<FeatureItem> SaveFeatureItemAsync(FeatureItem model) 
            => (await SaveFeatureItemsAsync(new[] { new Model.FeatureItem(model) })).SingleOrDefault();

        public async Task<IEnumerable<FeatureItem>> SaveFeatureItemsAsync(IEnumerable<Model.FeatureItem> models)
        {
            var saved = new List<FeatureItem>();
            foreach (var model in models)
            {
                if (model.Id == default)
                {
                    model.DateInserted = DateTime.UtcNow;

                    await _context.FeatureItems.AddAsync(model);
                    saved.Add(model.ToPublicModel());
                }
                else
                {
                    var existing = await _context.FeatureItems.AsTracking()
                        .Include(entity => entity.Examples)
                        .SingleOrDefaultAsync(entity => entity.Id == model.Id || entity.FeatureId == model.FeatureId && entity.Name == model.Name);

                    existing.DateUpdated = DateTime.UtcNow;
                    existing.Description = model.Description;
                    existing.IsDiscontinued = model.IsDiscontinued;
                    existing.IsHidden = model.IsHidden;
                    existing.IsNew = model.IsNew;
                    existing.Title = model.Title;
                    existing.XmlDocInfo = model.XmlDocInfo;
                    existing.XmlDocMetadata = model.XmlDocMetadata;
                    existing.XmlDocRemarks = model.XmlDocRemarks;
                    existing.XmlDocSourceObject = model.XmlDocSourceObject;
                    existing.XmlDocSummary = model.XmlDocSummary;
                    existing.XmlDocTabName = model.XmlDocTabName;

                    saved.Add(existing.ToPublicModel());
                }
            }

            await _context.SaveChangesAsync();
            return saved;
        }

        public async Task<IEnumerable<Tag>> SaveTagsAsync(IEnumerable<Tag> models)
        {
            var tags = new List<Tag>();
            foreach (var tagModel in models)
            {
                var tag = await SaveTagAsync(tagModel);
                tags.Add(tag);
            }
            await _context.SaveChangesAsync();
            return tags;
        }

        private async Task<Tag> SaveTagAsync(Tag model)
        {
            var existing = await _context.Tags.AsTracking()
                .Include(entity => entity.TagAssets)
                .SingleOrDefaultAsync(entity => entity.Id == model.Id || entity.Name == model.Name);

            if (existing is null)
            {
                var entity = new Model.Tag(model)
                {
                    DateInserted = DateTime.UtcNow
                };
                foreach (var asset in entity.TagAssets)
                {
                    asset.DateInserted = DateTime.UtcNow;
                }

                await _context.Tags.AddAsync(entity);
                return entity.ToPublicModel();
            }

            existing.DateUpdated = DateTime.UtcNow;
            existing.InstallerDownloads = model.InstallerDownloads;
            existing.IsPreRelease = model.IsPreRelease;

            return existing.ToPublicModel();
        }
    }
}
