using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rubberduck.Client;
using Rubberduck.Client.Abstract;
using Rubberduck.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RubberduckWebsite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(o =>
                {
                    // set the path for the authentication challenge
                    o.LoginPath = "/signin";
                    // set the path for the sign out
                    o.LogoutPath = "/signout";
                })
                .AddGitHub(o =>
                {
                    // derived from https://khalidabuhakmeh.com/github-openid-auth-aspnet-core-apps
                    o.ClientId = Configuration["ClientId"];
                    o.ClientSecret = Configuration["ClientSecret"];
                    o.CallbackPath = "/signin-github/";

                    // Grants access to read a user's profile data.
                    // https://docs.github.com/en/developers/apps/building-oauth-apps/scopes-for-oauth-apps
                    o.Scope.Add("read:user");
                    o.Scope.Add("read:org");

                    // need an access token to call GitHub Apis
                    o.Events.OnCreatingTicket += async context =>
                        {
                            if (context.AccessToken is not null)
                            {
                                context.Identity?.AddClaim(new Claim("access_token", context.AccessToken));

                                var github = new Octokit.GitHubClient(
                                    new Octokit.ProductHeaderValue("Rubberduck.Website"),
                                    new Octokit.Internal.InMemoryCredentialStore(new Octokit.Credentials(context.AccessToken)));

                                var orgs = await github.Organization.GetAllForUser(context.Identity.Name);
                                if (orgs.Any(org => org.Id == Organization.RubberduckOrgId))
                                {
                                    // todo: read user orgs from github
                                    // if user has rubberduck-org role, add role claim
                                    context.Identity?.AddClaim(new Claim(ClaimTypes.Role, "rubberduck-org"));
                                }
                            }
                        };
                });

            services.AddControllersWithViews();
            services.AddScoped<IAdminApiClient, AdminApiClient>();
            services.AddScoped<IPublicApiClient, PublicApiClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

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
    }
}
