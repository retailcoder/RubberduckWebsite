using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rubberduck.API.Services.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.ContentServices.Reader;
using Rubberduck.ContentServices.Writer;
using Rubberduck.Model.Internal;
using Rubberduck.SmartIndenter;
using RubberduckServices.Abstract;
using Rubberduck.ContentServices;
using Microsoft.EntityFrameworkCore;
using Rubberduck.ContentServices.XmlDoc.Abstract;
using Rubberduck.ContentServices.XmlDoc;

namespace Rubberduck.API.Extensions
{
    internal static class IServiceCollectionExtensions
    {
        public static void RegisterApiServices(this IServiceCollection services, IConfiguration configuration/*, IWebHostEnvironment environment*/)
        {
            var connectionString = configuration.GetConnectionString("RubberduckDb");
            services.AddDbContext<RubberduckDbContext>(options => options.UseSqlServer(connectionString));

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

            services.AddScoped<ICodeAnalysisXmlDocParser, CodeAnalysisXmlDocParser>();
            services.AddScoped<IParsingXmlDocParser, ParsingXmlDocParser>();
            services.AddScoped<IXmlDocMerge, XmlDocMerge>();

            // RubberduckServices
            services.AddScoped<ISyntaxHighlighterService, RubberduckServices.SyntaxHighlighterService>();
            services.AddScoped<IIndenterService, RubberduckServices.IndenterService>();
            services.AddScoped<ISimpleIndenter, SimpleIndenter>();
        }
    }
}
