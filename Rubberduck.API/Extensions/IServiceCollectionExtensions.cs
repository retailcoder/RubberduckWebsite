using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rubberduck.API.Services.Abstract;
using Rubberduck.ContentServices;
using Rubberduck.ContentServices.Reader;
using Rubberduck.ContentServices.Repository.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.ContentServices.Writer;
using Rubberduck.Model.Entity;
using RubberduckServices.Abstract;

namespace Rubberduck.API.Extensions
{
    internal static class IServiceCollectionExtensions
    {
        public static void RegisterApiServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var db = environment.IsDevelopment() ? "RubberduckDb_localdb" : "RubberduckDb";
            var connectionString = configuration.GetConnectionString(db);
            services.AddScoped<IReaderDbContext, DbContext>(provider => new DbContext(connectionString));
            services.AddScoped<IWriterDbContext, DbContext>(provider => new DbContext(connectionString));

            // API.Services
            services.AddScoped<IContentServices, Services.ContentServices>();
            services.AddScoped<IGitHubDataServices, Services.GitHubDataServices>();
            services.AddScoped<IXmlDocServices, Services.XmlDocServices>();

            // ContentServices
            services.AddScoped<IContentReaderService<Feature>, FeatureContentReaderService>();
            services.AddScoped<IContentReaderService<FeatureItem>, FeatureItemContentReaderService>();
            services.AddScoped<IContentReaderService<Tag>, ReleaseTagContentReaderService>();
            services.AddScoped<IContentReaderService<TagAsset>, TagAssetContentReaderService>();
            services.AddScoped<IContentReaderService<Example>, ExampleContentReaderService>();
            services.AddScoped<IContentReaderService<ExampleModule>, ExampleModuleContentReaderService>();

            services.AddScoped<IContentWriterService<Feature>, FeatureContentWriterService>();
            services.AddScoped<IContentWriterService<FeatureItem>, FeatureItemContentWriterService>();
            services.AddScoped<IContentWriterService<Example>, ExampleContentWriterService>();
            services.AddScoped<IContentWriterService<ExampleModule>, ExampleModuleContentWriterService>();
            services.AddScoped<IContentWriterService<Tag>, TagContentWriterService>();
            services.AddScoped<IContentWriterService<TagAsset>, TagAssetContentWriterService>();

            // RubberduckServices
            services.AddScoped<ISyntaxHighlighterService, RubberduckServices.SyntaxHighlighterService>();
            services.AddScoped<IIndenterService, RubberduckServices.IndenterService>();
        }
    }
}
