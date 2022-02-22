using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Octokit;
using Octokit.Internal;

namespace Rubberduck.API.Authorization
{
    /// <summary>
    /// Requires GitHub authentication and membership with rubberduck-vba organization.
    /// </summary>
    public class GitHubAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        /// <summary>
        /// Requires GitHub authentication and membership with rubberduck-vba organization.
        /// </summary>
        public GitHubAuthorizeAttribute()
        {
            AuthenticationSchemes = "GitHub";
        }

        /// <summary>
        /// Called early in the filter pipeline to confirm request is authorized.
        /// </summary>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            AuthenticateAsync(context).Wait();
        }

        /// <summary>
        /// Called early in the filter pipeline to confirm request is authorized.
        /// </summary>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await AuthenticateAsync(context);
        }

        private static async Task AuthenticateAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var accessToken = await context.HttpContext.GetTokenAsync("access_token");
                var github = new GitHubClient(new ProductHeaderValue("AspNetCoreGitHubAuth"), new InMemoryCredentialStore(new Credentials(accessToken)));

                var orgs = await github.Organization.GetAllForCurrent();

                if (orgs.Any(org => org.Name == "rubberduck-vba" && org.HtmlUrl == "https://rubberduckvba.com"))
                {
                    context.Result = new OkResult();
                }
                else
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
