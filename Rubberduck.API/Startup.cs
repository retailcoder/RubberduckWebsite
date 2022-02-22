using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Rubberduck.API.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Text.Json;
using Octokit.Internal;
using Octokit;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Rubberduck.API
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rubberduck.API", Version = "v1" });
                //c.AddSecurityDefinition("GitHub", new OpenApiSecurityScheme
                //{
                //    Description = "GitHub",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.OAuth2,
                //    Flows = new OpenApiOAuthFlows
                //    {
                //        ClientCredentials = new OpenApiOAuthFlow
                //        {
                //            AuthorizationUrl = new Uri("https://github.com/login/oauth/authorize/", UriKind.Absolute),
                //            TokenUrl = new Uri("https://github.com/login/oauth/access_token", UriKind.Absolute),
                //            Scopes = new Dictionary<string, string>
                //            {
                //                ["read:user"] = "Read access to GitHub user profile",
                //                ["read:org"] = "Read access to GitHub Organization memberships",
                //            },
                //                                    },
                //    },
                //    BearerFormat = "Bearer <token>",
                //});

                c.OperationFilter<AddHeaderParam>();
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
            services.AddDistributedMemoryCache(options =>
            {
                /* TODO? */
            });
            services.AddSession(options =>
            {
                /* TODO? */
            });
            services.RegisterApiServices(Configuration);
            services.AddLogging(ConfigureLogging);

            //services
                //.AddAuthentication(o =>
                //{
                //    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //})
                //.AddCookie(o =>
                //{
                //    // set the path for the authentication challenge
                //    o.LoginPath = "/signin";
                //    // set the path for the sign out
                //    o.LogoutPath = "/signout";
                //})
                //.AddGitHub(o =>
                //{
                //    // derived from https://khalidabuhakmeh.com/github-openid-auth-aspnet-core-apps
                //    o.ClientId = Configuration["ClientId"];
                //    o.ClientSecret = Configuration["ClientSecret"];
                //    o.CallbackPath = "/signin-github/";

                //    // Grants access to read a user's profile data.
                //    // https://docs.github.com/en/developers/apps/building-oauth-apps/scopes-for-oauth-apps
                //    o.Scope.Add("read:user");
                //    o.Scope.Add("read:org");

                //    // Optional
                //    // if you need an access token to call GitHub Apis
                //    o.Events.OnCreatingTicket += async context =>
                //    {
                //        if (context.AccessToken is not null)
                //        {
                //            context.Identity?.AddClaim(new Claim("access_token", context.AccessToken));

                //            var github = new GitHubClient(
                //                new Octokit.ProductHeaderValue("Rubberduck.API"),
                //                new InMemoryCredentialStore(new Credentials(context.AccessToken)));

                //            var orgs = await github.Organization.GetAllForUser(context.Identity.Name);
                //            if (orgs.Any(org => org.Id == Model.Common.Organization.RubberduckOrgId))
                //            {
                //                // if user has rubberduck-org role, add role claim
                //                context.Identity?.AddClaim(new Claim(ClaimTypes.Role, "rubberduck-org"));
                //            }
                //        }
                //    };
                //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rubberduck.API v1"));
            }

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(policy => 
            {
                policy.AllowAnyOrigin();
            });

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/signout", async ctx =>
                {
                    await ctx.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new AuthenticationProperties
                        {
                            RedirectUri = "/"
                        });
                });
            });

        }

        private void ConfigureLogging(ILoggingBuilder builder)
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Trace);
        }
    }

    public class AddHeaderParam : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var requirement = new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "GitHub",
                        },
                        Scheme = "GitHub",
                        Name = "GitHub",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                },
            };
            
            operation.Security.Add(requirement);
        }
    }
}
