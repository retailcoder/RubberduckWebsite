using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rubberduck.Client.Abstract;
using Rubberduck.Model.Entities;
using RubberduckWebsite.Models;

namespace RubberduckWebsite.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger _logger;
        private readonly IAdminApiClient _apiClient;
        private readonly IWebHostEnvironment _webHost;

        public AdminController(ILogger<AdminController> logger, IAdminApiClient apiClient, IWebHostEnvironment webHost)
        {
            _logger = logger;
            _apiClient = apiClient;
            _webHost = webHost;
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

        [HttpGet]
        [Route("Edit/{name}")]
        public async Task<IActionResult> EditFeature([FromRoute]string name)
        {
            var topLevelFeatures = await _apiClient.GetFeaturesAsync();
            var features = topLevelFeatures.Concat(topLevelFeatures.SelectMany(e => e.SubFeatures)).ToList();

            var feature = features.SingleOrDefault(e => e.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (feature is null)
            {
                return NotFound();
            }

            var viewModel = new EditFeatureViewModel(feature, features);
            return View("Feature", viewModel);
        }

        [HttpGet]
        [Route("NewFeature")]
        public async Task<IActionResult> NewFeature()
        {
            var topLevelFeatures = await _apiClient.GetFeaturesAsync();
            var features = topLevelFeatures.Concat(topLevelFeatures.SelectMany(e => e.SubFeatures)).ToList();

            var feature = new Feature
            {
                IsNew = true,
                IsHidden = true,
                SortOrder = 1,
                ParentFeature = null,
                ParentId = null,
                Name = "NewFeature",
                Title = "New Top-Level Feature",
                ElevatorPitch = "Summarize the feature in 2 or 3 sentences here.",
                Description = "Use Markdown syntax to describe the feature in under 30K characters.",
                XmlDocSource = null,
            };
            var viewModel = new EditFeatureViewModel(feature, features);
            return View("Feature", viewModel);
        }

        [HttpGet]
        [Route("{parentName}/Sub")]
        public async Task<IActionResult> NewSubFeature([FromRoute] string parentName)
        {
            var parent = await _apiClient.GetFeatureAsync(parentName);
            if (parent is null)
            {
                return NotFound();
            }

            var features = await _apiClient.GetFeaturesAsync();
            var subFeature = new Feature
            {
                IsNew = true,
                IsHidden = true,
                SortOrder = 1,
                ParentFeature = parent,
                ParentId = parent.Id,
                Name = "NewSubFeature",
                Title = $"New Sub-Feature of {parent.Name}",
                ElevatorPitch = "Summarize the sub-feature in 2 or 3 sentences here.",
                Description = "Use Markdown syntax to describe the sub-feature in under 30K characters.",
                XmlDocSource = null,
            };
            var viewModel = new EditFeatureViewModel(subFeature, features);
            return View("Feature", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveFeature(EditFeatureViewModel vm)
        {
            try
            {
                await _apiClient.SaveFeatureAsync(vm.GetModel());
                return View("Feature", vm);
            }
            catch(Exception exception)
            {
                return Problem(exception.ToString());
            }
        }


        [HttpPost]
        public async Task<IActionResult> UploadScreenshot(string featureName, IFormFile screenshotFile)
        {
            var feature = await _apiClient.GetFeatureAsync(featureName);
            if (feature is null)
            {
                return BadRequest($"Cannot upload screenshot for feature '{featureName}'.");
            }

            var path = Path.Combine(_webHost.WebRootPath, "images\\features");
            var extension = Path.GetExtension(screenshotFile.FileName);
            if (string.IsNullOrWhiteSpace(extension))
            {
                return BadRequest($"File name has no extension.");
            }
            var fileName = Path.Combine(path, $"{feature.Name}{extension}");

            try
            {
                var info = new FileInfo(fileName);
                if (info.Exists)
                {
                    info.Delete();
                }

                using (var stream = new FileStream(fileName, FileMode.CreateNew))
                {
                    screenshotFile.CopyTo(stream);
                }

                return await EditFeature(feature.Name);
            }
            catch (Exception e)
            {
                return Problem(e.ToString());
            }
        }
    }
}
