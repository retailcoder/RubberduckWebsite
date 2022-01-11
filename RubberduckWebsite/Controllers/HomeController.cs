using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rubberduck.Client.Abstract;
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

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
