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
                c.AddSecurityDefinition("GitHub", new OpenApiSecurityScheme
                {
                    Description = "GitHub",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://github.com/login/oauth/authorize/", UriKind.Absolute),
                            TokenUrl = new Uri("https://github.com/login/oauth/access_token", UriKind.Absolute),
                            Scopes = new Dictionary<string, string>
                            {
                                ["read:user"] = "Read access to GitHub user profile",
                                ["read:org"] = "Read access to GitHub Organization memberships",
                            },
                                                    },
                    },
                    BearerFormat = "Bearer <token>",
                });

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

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = "GitHub";
                })
                .AddCookie()
                .AddOAuth("GitHub", options =>
                {
                    options.ClientId = Configuration["GitHub:ClientId"];
                    options.ClientSecret = Configuration["GitHub:ClientSecret"];
                    options.CallbackPath = new PathString("/api/auth/login/");
                    options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize/";
                    options.TokenEndpoint = "https://github.com/login/oauth/access_token/";
                    options.UserInformationEndpoint = "https://api.github.com/user/";
                    options.SaveTokens = true;
                    options.Scope.Add("read:user");
                    options.Scope.Add("read:org");
                    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                    options.ClaimActions.MapJsonKey("urn:github:login", "login");
                    options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
                    options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");
                    options.Events = new OAuthEvents
                    {
                        OnCreatingTicket = async context =>
                        {
                            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                            
                            var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                            response.EnsureSuccessStatusCode();
                            
                            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                            context.RunClaimActions(json.RootElement);

                            if (!context.Identity.IsAuthenticated)
                            {
                                context.NoResult();
                            }
                        },
                        OnRedirectToAuthorizationEndpoint = async context =>
                        {
                            var session = context.HttpContext.Session;
                            await Task.CompletedTask;
                        },
                        OnTicketReceived = async context =>
                        {
                            var response = context.Response;
                            await Task.CompletedTask;
                        },
                        OnAccessDenied = async context =>
                        {
                            var response = context.Response;
                            await Task.CompletedTask;
                        }
                    };
                });
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
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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
