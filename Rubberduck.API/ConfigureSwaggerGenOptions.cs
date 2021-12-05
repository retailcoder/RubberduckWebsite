using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Rubberduck.API
{
    /// <summary>
    /// Configures Swagger generation options.
    /// </summary>
    public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        /// <summary>
        /// Invoked to configure a SwaggerGenOptions instance.
        /// </summary>
        public void Configure(SwaggerGenOptions options)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var xmldocLocation = Path.Combine(Path.GetDirectoryName(assembly.Location), $"{assembly.GetName().Name}.xml");
            options.IncludeXmlComments(xmldocLocation);
        }
    }
}
