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
                    entity.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
                    entity.HasIndex(e => e.Name).IsUnique();
                    entity.HasMany(e => e.FeatureItems).WithOne(e => e.Feature).HasForeignKey(e => e.FeatureId).IsRequired();
                    entity.HasOne(e => e.ParentFeature).WithMany(e => e.SubFeatures).HasForeignKey(e => e.ParentId).IsRequired(false);
                })
                .Entity<FeatureItemEntity>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
                    entity.HasIndex(e => new { e.FeatureId, e.Name}).IsUnique();
                    entity.HasMany(e => e.Examples).WithOne(e => e.FeatureItem).HasForeignKey(e => e.FeatureItemId).IsRequired();
                })
                .Entity<ExampleEntity>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
                    entity.HasMany(e => e.Modules).WithOne(e => e.Example).HasForeignKey(e => e.ExampleId).IsRequired();
                })
                .Entity<TagEntity>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasIndex(e => e.Name).IsUnique();
                    entity.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
                    entity.HasMany(e => e.TagAssets).WithOne().HasForeignKey(e => e.TagId).IsRequired();
                })
                .Entity<TagAssetEntity>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasIndex(e => new { e.TagId, e.Name }).IsUnique();
                    entity.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
                });
        }
    }
}
