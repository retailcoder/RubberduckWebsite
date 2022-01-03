using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Rubberduck.Client.Abstract;
using RubberduckWebsite.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RubberduckWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPublicApiClient _api;

        public HomeController(ILogger<HomeController> logger, IPublicApiClient api)
        {
            _logger = logger;
            _api = api;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"Executing action {context.ActionDescriptor.DisplayName}. Request path: {Request.Path}");
        }

        public async Task<IActionResult> Index()
        {
            var model = await GetViewModelAsync();
            return View(model);
        }

        private async Task<HomeViewModel> GetViewModelAsync()
        {
            var latestTags = await _api.GetLatestTagsAsync();
            var features = await _api.GetFeaturesAsync();

            return new HomeViewModel(latestTags, features);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
