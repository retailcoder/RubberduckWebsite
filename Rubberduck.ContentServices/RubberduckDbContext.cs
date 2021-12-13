using Microsoft.EntityFrameworkCore;
using Rubberduck.Model.DTO;

namespace Rubberduck.ContentServices
{
    public class RubberduckDbContext : DbContext
    {
        public RubberduckDbContext(DbContextOptions<RubberduckDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FeatureEntity> Features { get; set; }
        public virtual DbSet<FeatureItemEntity> FeatureItems { get; set; }
        public virtual DbSet<ExampleEntity> Examples { get; set; }
        public virtual DbSet<ExampleModuleEntity> ExampleModules { get; set; }
        public virtual DbSet<TagEntity> Tags { get; set; }
        public virtual DbSet<TagAssetEntity> TagAssets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<FeatureEntity>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasIndex(e => e.Name).IsUnique(true);
                    entity.HasOne(e => e.ParentFeature).WithMany(e => e.SubFeatures).HasForeignKey(e => e.ParentId).IsRequired(false);
                    entity.HasMany(e => e.FeatureItems).WithOne(e => e.Feature).HasForeignKey(e => e.FeatureId);
                })
                .Entity<FeatureItemEntity>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasOne(e => e.Feature).WithMany(e => e.FeatureItems).HasForeignKey(e => e.FeatureId);
                    entity.HasMany(e => e.Examples).WithOne(e => e.FeatureItem).HasForeignKey(e => e.FeatureItemId);
                    entity.HasOne(e => e.TagAsset).WithMany(e => e.FeatureItems).HasForeignKey(e => e.TagAssetId).IsRequired(false);
                })
                .Entity<ExampleEntity>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasOne(e => e.FeatureItem).WithMany(e => e.Examples).HasForeignKey(e => e.FeatureItemId);
                    entity.HasMany(e => e.Modules).WithOne(e => e.Example).HasForeignKey(e => e.ExampleId);
                })
                .Entity<TagEntity>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasMany(e => e.TagAssets).WithOne(e => e.Tag).HasForeignKey(e => e.TagId);
                })
                .Entity<TagAssetEntity>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasOne(e => e.Tag).WithMany(e => e.TagAssets).HasForeignKey(e => e.TagId);
                    entity.HasMany(e => e.FeatureItems).WithOne(e => e.TagAsset).HasForeignKey(e => e.TagAssetId).IsRequired(false);
                });
        }
    }
}
