using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rubberduck.Client.Abstract;
using RubberduckWebsite.Models;

namespace RubberduckWebsite.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger _logger;
        private readonly IAdminApiClient _apiClient;

        public AdminController(ILogger<AdminController> logger, IAdminApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var tags = await _apiClient.GetLatestTagsAsync();
            var tagTimestamp = tags.Any() ? tags.Max(tag => tag.DateUpdated ?? tag.DateInserted) : default(DateTime?);

            var features = await _apiClient.GetFeaturesAsync();
            
            var vm = new AdminViewModel(tagTimestamp, features);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTagMetadata()
        {
            try
            {
                var result = await _apiClient.UpdateTagMetadataAsync();
                return Ok(result);
            }
            catch(TaskCanceledException)
            {
                return Problem("Timeout expired.", statusCode:408);
            }
            catch(Exception exception)
            {
                return Problem(exception.ToString());
            }
        }
    }
}
