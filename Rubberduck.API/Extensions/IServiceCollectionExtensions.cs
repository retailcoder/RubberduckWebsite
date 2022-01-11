using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Rubberduck.ContentServices;
using Rubberduck.ContentServices.XmlDoc.Abstract;
using Rubberduck.ContentServices.XmlDoc;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.ContentServices.Service;
using Rubberduck.SmartIndenter;
using Rubberduck.API.Services.Abstract;
using Rubberduck.API.Services;
using RubberduckServices.Abstract;
using RubberduckServices;

namespace Rubberduck.API.Extensions
{
    internal static class IServiceCollectionExtensions
    {
        public static void RegisterApiServices(this IServiceCollection services, IConfiguration configuration/*, IWebHostEnvironment environment*/)
        {
            var connectionString = configuration.GetConnectionString("RubberduckDb");
            services.AddDbContext<RubberduckDbContext>(options => options.UseSqlServer(connectionString));

            // API.Services
            services.AddScoped<IContentService, ContentService>();
            services.AddScoped<IGitHubDataServices, GitHubDataServices>();
            services.AddScoped<IXmlDocServices, XmlDocServices>();

            // ContentServices

            services.AddScoped<ICodeAnalysisXmlDocParser, CodeAnalysisXmlDocParser>();
            services.AddScoped<IParsingXmlDocParser, ParsingXmlDocParser>();
            services.AddScoped<IXmlDocMerge, XmlDocMerge>();

            // RubberduckServices
            services.AddScoped<ISyntaxHighlighterService, SyntaxHighlighterService>();
            services.AddScoped<IIndenterService, IndenterService>();
            services.AddScoped<ISimpleIndenter, SimpleIndenter>();
        }
    }
}
