using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rubberduck.API.DTO;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Entity;
using RubberduckServices.Abstract;

namespace Rubberduck.API.Controllers
{
    /// <summary>
    /// Exposes endpoints providing the website's dynamic content.
    /// </summary>
    [ApiController]
    [Route("[Controller]")]
    public class PublicController : ControllerBase
    {
        private readonly ILogger<PublicController> _logger;
        private readonly IContentReaderService<Feature> _featuresReader;
        private readonly IContentReaderService<FeatureItem> _featureItemsReader;
        private readonly IContentReaderService<Tag> _tagsReader;
        private readonly IIndenterService _indenterService;

        /// <summary>
        /// Creates a controller that exposes endpoints providing the website's dynamic content.
        /// </summary>
        public PublicController(ILogger<PublicController> logger,
            IContentReaderService<Feature> featuresReader,
            IContentReaderService<FeatureItem> featureItemsReader,
            IContentReaderService<Tag> tagsReader,
            IIndenterService indenterService)
        {
            _logger = logger;
            _featuresReader = featuresReader;
            _featureItemsReader = featureItemsReader;
            _tagsReader = tagsReader;
            _indenterService = indenterService;
        }

        /// <summary>
        /// Gets all features, sub-features, and feature items.
        /// </summary>
        [HttpGet]
        [Route("Features")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.DTO.Feature>), 200)]
        public async Task<ActionResult<IEnumerable<Model.DTO.Feature>>> GetFeaturesAsync()
        {
            try
            {
                var features = await _featuresReader.GetAllAsync().ContinueWith(t => t.Result.Select(Feature.ToDTO));
                return Ok(features);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while retrieving the requested objects.", statusCode: 500);
            }
        }

        /// <summary>
        /// Gets the specified feature item, including its examples and their respective modules.
        /// </summary>
        /// <param name="id">The internal ID of the feature item to get.</param>
        [HttpGet]
        [Route("FeatureItem/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.DTO.FeatureItem), 200)]
        public async Task<ActionResult<FeatureItem>> GetFeatureItem([FromRoute]int id)
        {
            try
            {
                var item = await _featureItemsReader.GetByIdAsync(id);
                if (item is null)
                {
                    return NotFound();
                }
                return Ok(FeatureItem.ToDTO(item));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while retrieving the requested objects.", statusCode: 500);
            }
        }

        /// <summary>
        /// Gets the latest release and prerelease tags.
        /// </summary>
        [HttpGet]
        [Route("Tags")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.DTO.Tag>), 200)]
        public async Task<ActionResult<IEnumerable<Model.DTO.Tag>>> GetLatestTagsAsync()
        {
            try
            {
                var tags = await _tagsReader
                    .GetAllAsync() // FIXME behavior is overridden at the repository level to return the latest tags
                    .ContinueWith(t => t.Result.Select(Tag.ToDTO));
                return Ok(tags);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while retrieving the latest tags.", statusCode: 500);
            }
        }

        /// <summary>
        /// Gets the xmldoc assets for the specified tag.
        /// </summary>
        /// <param name="id">The internal ID of the tag to get assets for.</param>
        [HttpGet]
        [Route("TagAssets/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.DTO.TagAsset>), 200)]
        public async Task<ActionResult<IEnumerable<Model.DTO.TagAsset>>> GetTagAssets([FromRoute]int id)
        {
            try
            {
                var tag = await _tagsReader.GetByIdAsync(id);
                if (tag is null)
                {
                    return NotFound();
                }
                var assets = tag.Assets.Select(TagAsset.ToDTO);
                return Ok(assets);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while retrieving assets for the specified tag.", statusCode: 500);
            }
        }

        /// <summary>
        /// Gets the supplied code, indented as per specified settings.
        /// </summary>
        /// <param name="viewModel">The indenter request details.</param>
        /// <returns>An array of string, each element being an indented physical line of code.</returns>
        [HttpPost]
        [Route("Indent")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string[]), 200)]
        public async Task<ActionResult<string>> IndentAsync([FromBody]IndenterViewModel viewModel)
        {
            try
            {
                var result = await _indenterService.IndentAsync(viewModel.Code, viewModel);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while indenting the provided code.", statusCode: 500);
            }
        }
    }
}
