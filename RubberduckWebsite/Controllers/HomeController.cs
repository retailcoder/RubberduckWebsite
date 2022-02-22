using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rubberduck.Client.Abstract;
using Rubberduck.Model;
using RubberduckWebsite.Controllers.Abstract;
using RubberduckWebsite.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RubberduckWebsite.Controllers
{
    public class HomeController : PublicApiClientController<HomeViewModel>
    {
        public HomeController(ILogger<HomeController> logger, IPublicApiClient apiClient)
            : base(logger, apiClient) { }

        protected async override Task<HomeViewModel> GetViewModelAsync()
        {
            var latestTags = await ApiClient.GetLatestTagsAsync();
            var features = await ApiClient.GetFeaturesAsync();

            return new HomeViewModel(latestTags, features);
        }

        [HttpGet]
        [Route("/signin")]
        public ActionResult Signin()
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/"
            }, "GitHub");
        }

        [HttpPost]
        [Route("Home/Search")]
        public async Task<ActionResult<SearchResultsViewModel>> SearchContentAsync(string search)
        {
            if (search is null)
            {
                return BadRequest();
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}/";

            var vm = new SearchViewModel(search);
            var result = await ApiClient.SearchContentAsync(vm);

            foreach (var match in result.Results)
            {
                match.Url = $"{baseUrl}{match.Url}";
            }

            return View("SearchResults", result);
        }

        [HttpGet]
        [Route("Home/Downloads")]
        public async Task<JsonResult> GetDownloadLinks()
        {
            var latestTags = await ApiClient.GetLatestTagsAsync();
            var result = latestTags
                .Select(e => new { 
                    tag = e.Name, 
                    url = e.InstallerDownloadUrl, 
                    downloads = e.InstallerDownloads, 
                    isPreRelease = e.IsPreRelease 
                }).OrderBy(e => e.isPreRelease);

            return new JsonResult(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
