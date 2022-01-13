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

            return entity?.ToPublicModel();
        }

        public async Task<Tag> GetNextTagAsync()
        {
            var entity = await _context.Tags
                .Where(entity => entity.IsPreRelease)
                .OrderByDescending(entity => entity.DateCreated)
                .Include(entity => entity.TagAssets)
                .FirstOrDefaultAsync();

            return entity?.ToPublicModel();
        }

        public async Task<Example> SaveExampleAsync(Example model)
        {
            var existing = await _context.Examples.AsTracking()
                .SingleOrDefaultAsync(entity => entity.Id == model.Id);

            if (existing is null)
            {
                var entity = new Model.Example(model)
                {
                    DateInserted = DateTime.UtcNow
                };

                await _context.Examples.AddAsync(entity);
                return entity.ToPublicModel();
            }

            existing.DateUpdated = DateTime.UtcNow;
            existing.Description = model.Description;
            existing.SortOrder = model.SortOrder;

            await _context.SaveChangesAsync();
            return existing.ToPublicModel();
        }

        public async Task<ExampleModule> SaveExampleModuleAsync(ExampleModule model)
        {
            var existing = await _context.ExampleModules.AsTracking()
                .SingleOrDefaultAsync(entity => entity.Id == model.Id);

            if (existing is null)
            {
                var entity = new Model.ExampleModule(model)
                {
                    DateInserted = DateTime.UtcNow
                };

                await _context.ExampleModules.AddAsync(entity);
                return entity.ToPublicModel();
            }

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
            var existing = await _context.Features.AsTracking()
                .SingleOrDefaultAsync(entity => entity.Id == model.Id || entity.Name == model.Name);

            if (existing is null)
            {
                var entity = new Model.Feature(model)
                {
                    DateInserted = DateTime.UtcNow
                };

                await _context.Features.AddAsync(entity);
                return entity.ToPublicModel();
            }

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
                var existing = await _context.FeatureItems.AsTracking()
                    .SingleOrDefaultAsync(entity => entity.Id == model.Id || entity.FeatureId == model.FeatureId && entity.Name == model.Name);

                if (existing is null)
                {
                    model.DateInserted = DateTime.UtcNow;
                    await SaveExamplesAsync(model);
                    await _context.FeatureItems.AddAsync(model);
                    saved.Add(model.ToPublicModel());
                }
                else
                {
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

                    await SaveExamplesAsync(model);

                    saved.Add(existing.ToPublicModel());
                }
            }

            await _context.SaveChangesAsync();
            return saved;
        }

        private async Task SaveExamplesAsync(Model.FeatureItem item)
        {
            foreach (var model in item.Examples.OrderBy(e => e.SortOrder).Select((e, i) => (example: e, index: i + 1)))
            {
                var existing = await _context.Examples.AsTracking()
                    .SingleOrDefaultAsync(entity => entity.Id == model.example.Id || entity.FeatureItemId == model.example.FeatureItemId && entity.SortOrder == model.index);

                if (existing is null)
                {
                    model.example.DateInserted = DateTime.UtcNow;
                    model.example.SortOrder = model.index;

                    await SaveExampleModulesAsync(model.example);
                    continue;
                }

                existing.DateUpdated = DateTime.UtcNow;
                existing.Description = model.example.Description;
                existing.SortOrder = model.index;

                await SaveExampleModulesAsync(model.example);
            }
        }

        private async Task SaveExampleModulesAsync(Model.Example item)
        {
            foreach (var model in item.Modules.OrderBy(e => e.SortOrder).Select((e, i) => (module: e, index: i + 1)))
            {
                var existing = await _context.ExampleModules.AsTracking()
                    .SingleOrDefaultAsync(entity => entity.Id == model.module.Id || entity.ExampleId == model.module.ExampleId && entity.SortOrder == model.index);

                if (existing is null)
                {
                    model.module.DateInserted = DateTime.UtcNow;
                    model.module.SortOrder = model.index;
                    model.module.HtmlContent = model.module.HtmlContent ?? "(error parsing code example from source xmldoc)";
                    continue;
                }

                existing.DateUpdated = DateTime.UtcNow;
                existing.Description = model.module.Description;
                existing.HtmlContent = model.module.HtmlContent ?? "(error parsing code example from source xmldoc)";
                existing.ModuleName = model.module.ModuleName;
                existing.ModuleTypeId = model.module.ModuleTypeId;
                existing.SortOrder = model.index;
            }
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
            
            // no need to update assets, asset download url should be immutable.

            return existing.ToPublicModel();
        }
    }
}
