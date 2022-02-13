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
        [Route("{controller}/UpdateTagMetadata")]
        public async Task<IActionResult> UpdateTagMetadata()
        {
            try
            {
                _logger.LogInformation("Beginning tag metadata update task...");
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
        [Route("{controller}/EditFeature/{name}")]
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
        [Route("{controller}/NewFeature")]
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
        [Route("{controller}/NewSubFeature")]
        public async Task<IActionResult> NewSubFeature([FromQuery] string parent)
        {
            if (string.IsNullOrWhiteSpace(parent))
            {
                return BadRequest();
            }

            var features = (await _apiClient.GetFeaturesAsync()).ToDictionary(e => e.Name, e => e);
            if (!features.ContainsKey(parent))
            {
                return NotFound();
            }

            var feature = features[parent];
            var subFeature = new Feature
            {
                IsNew = true,
                IsHidden = true,
                SortOrder = 1,
                ParentFeature = feature,
                ParentId = feature.Id,
                Name = "NewSubFeature",
                Title = $"New Sub-Feature of {feature.Name}",
                ElevatorPitch = "Summarize the sub-feature in 2 or 3 sentences here.",
                Description = "Use Markdown syntax to describe the sub-feature in under 30K characters.",
                XmlDocSource = null,
            };
            var viewModel = new EditFeatureViewModel(subFeature, features.Values);
            return View("Feature", viewModel);
        }

        [HttpGet]
        [Route("{controller}/NewFeatureItem")]
        public async Task<IActionResult> NewFeatureItem([FromQuery] string parent)
        {
            if (string.IsNullOrWhiteSpace(parent))
            {
                return BadRequest();
            }

            var features = (await _apiClient.GetFeaturesAsync()).ToDictionary(e => e.Name, e => e);
            if (!features.ContainsKey(parent))
            {
                return NotFound();
            }

            var feature = features[parent];
            var item = new FeatureItem
            {
                IsNew = true,
                IsHidden = true,
                Feature = feature,
                FeatureId = feature.Id,
                Name = "NewFeatureItem",
                Title = $"New Item under {feature.Name}",
                Description = "Use Markdown syntax to describe the item in under 30K characters.",
            };
            var viewModel = new EditFeatureItemViewModel(item, features.Values);
            return View("FeatureItem", viewModel);
        }

        [HttpPost]
        [Route("{controller}/EditFeature/Save")]
        public async Task<IActionResult> SaveFeature([FromBody] EditFeatureViewModel vm)
        {
            try
            {
                var features = await _apiClient.GetFeaturesAsync();
                var model = await _apiClient.SaveFeatureAsync(vm.GetModel(features));
                return Ok(new EditFeatureViewModel(model, features));
            }
            catch(Exception exception)
            {
                return Problem(exception.ToString());
            }
        }

        [HttpPost]
        [Route("{controller}/EditFeature/Delete")]
        public async Task<IActionResult> DeleteFeature([FromBody] EditFeatureViewModel vm)
        {
            var name = vm?.Name;
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest();
            }

            try
            {
                var model = await _apiClient.GetFeatureAsync(name);
                if (model is null)
                {
                    return NotFound();
                }
                var result = await _apiClient.DeleteFeatureAsync(model);
                return Ok(result);
            }
            catch (Exception exception)
            {
                return Problem(exception.ToString());
            }
        }

        [HttpPost]
        [Route("{controller}/EditFeature/DeleteFeatureItem")]
        public async Task<IActionResult> DeleteFeatureItem([FromBody] EditFeatureViewModel vm)
        {
            var name = vm?.Name;
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest();
            }

            try
            {
                var model = await _apiClient.GetFeatureItemAsync(name);
                if (model is null)
                {
                    return NotFound();
                }
                if (model.Feature.IsProtected)
                {
                    return BadRequest();
                }

                var result = await _apiClient.DeleteFeatureItemAsync(model);
                return Ok(result);
            }
            catch (Exception exception)
            {
                return Problem(exception.ToString());
            }
        }

        [HttpPost]
        [Route("{controller}/EditFeature/MarkdownPreview")]
        public async Task<IActionResult> MarkdownPreview([FromBody] MarkdownPreviewViewModel vm)
        {
            var html = new MarkdownSharp.Markdown().Transform(vm.MarkdownContent);
            return await Task.FromResult(Ok(html));
        }

        [HttpPost]
        [Route("{controller}/EditFeature/UploadScreenshot")]
        public async Task<IActionResult> UploadScreenshot()
        {
            var featureName = Request.Form["featureName"];
            var screenshotFile = Request.Form.Files[0];

            var feature = await _apiClient.GetFeatureAsync(featureName);
            if (feature is null)
            {
                return BadRequest($"Cannot upload screenshot for feature '{featureName}' at this time.");
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

                return Ok();
            }
            catch (Exception exception)
            {
                return Problem(exception.ToString());
            }
        }
    }
}
