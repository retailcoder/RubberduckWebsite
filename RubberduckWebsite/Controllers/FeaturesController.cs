﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rubberduck.Client.Abstract;
using RubberduckWebsite.Controllers.Abstract;
using RubberduckWebsite.Models;

namespace RubberduckWebsite.Controllers
{
    [Route("{controller}")]
    public class FeaturesController : PublicApiClientController<FeaturesViewModel>
    {
        public FeaturesController(ILogger<FeaturesController> logger, IPublicApiClient apiClient)
            : base(logger, apiClient) { }

        protected override async Task<FeaturesViewModel> GetViewModelAsync()
        {
            var features = await ApiClient.GetFeaturesAsync();
            return new FeaturesViewModel(features);
        }

        /// <summary>
        /// Gets the summary page for the specified feature, listing items and sub-feature summaries.
        /// </summary>
        [HttpGet]
        [Route("{name}")]
        public async Task<ActionResult<FeatureDetailsViewModel>> Details([FromRoute]string name)
        {
            var feature = await ApiClient.GetFeatureAsync(name);
            if (feature is null)
            {
                return NotFound();
            }

            var vm = new FeatureDetailsViewModel(feature);
            return View(vm);
        }

        /// <summary>
        /// Gets the details page for the specified feature item.
        /// </summary>
        [HttpGet]
        [Route("Details/{name}")]
        public async Task<ActionResult<FeatureDetailsViewModel>> Item([FromRoute]string name)
        {
            var item = await ApiClient.GetFeatureItemAsync(name);
            if (item is null)
            {
                return NotFound();
            }

            // TODO

            return null;
        }
    }
}