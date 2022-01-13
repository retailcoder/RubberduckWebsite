using Microsoft.EntityFrameworkCore;
using Rubberduck.ContentServices.Model;

namespace Rubberduck.ContentServices
{
    public class RubberduckDbContext : DbContext
    {
        public RubberduckDbContext(DbContextOptions<RubberduckDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Feature> Features { get; set; }
        public virtual DbSet<FeatureItem> FeatureItems { get; set; }
        public virtual DbSet<Example> Examples { get; set; }
        public virtual DbSet<ExampleModule> ExampleModules { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TagAsset> TagAssets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Feature>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
                    entity.HasIndex(e => e.Name).IsUnique();
                    entity.HasMany(e => e.FeatureItems).WithOne(e => e.Feature).HasForeignKey(e => e.FeatureId).IsRequired();
                    entity.HasOne(e => e.ParentFeature).WithMany(e => e.SubFeatures).HasForeignKey(e => e.ParentId).IsRequired(false);
                })
                .Entity<FeatureItem>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
                    entity.HasIndex(e => new { e.FeatureId, e.Name}).IsUnique();
                    entity.HasMany(e => e.Examples).WithOne(e => e.FeatureItem).HasForeignKey(e => e.FeatureItemId).IsRequired();
                })
                .Entity<Example>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
                    entity.HasMany(e => e.Modules).WithOne(e => e.Example).HasForeignKey(e => e.ExampleId).IsRequired();
                })
                .Entity<Tag>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasIndex(e => e.Name).IsUnique();
                    entity.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
                    entity.HasMany(e => e.TagAssets).WithOne().HasForeignKey(e => e.TagId).IsRequired();
                })
                .Entity<TagAsset>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasIndex(e => new { e.TagId, e.Name }).IsUnique();
                    entity.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();
                });
        }
    }
}
