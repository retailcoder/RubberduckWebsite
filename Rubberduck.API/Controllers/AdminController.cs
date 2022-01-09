using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rubberduck.API.Authorization;
using Rubberduck.API.Services.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model.Entities;

namespace Rubberduck.API.Controllers.Authenticated
{
    /// <summary>
    /// Exposes endpoints providing an interface to manipulate the website's dynamic content.
    /// </summary>
    [ApiController]
    //[Authorize]
    //[TypeFilter(typeof(GitHubAuthorizeAttribute))]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IContentService _contentService;
        private readonly IXmlDocServices _xmlDocService;

        /// <summary>
        /// Creates a controller that exposes endpoints providing an interface to manipulate the website's dynamic content.
        /// </summary>
        public AdminController(ILogger<AdminController> logger, 
            IContentService contentService, 
            IXmlDocServices xmlDocService)
        {
            _logger = logger;
            _contentService = contentService;
            _xmlDocService = xmlDocService;
        }

        /// <summary>
        /// Creates a new feature (or sub-feature), or updates an existing one.
        /// </summary>
        /// <param name="dto">The feature (or sub-feature) to save.</param>
        [HttpPost]
        [Route("SaveFeature")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Feature>), 200)]
        public async Task<ActionResult<Feature>> SaveAsync(Feature dto)
        {
            try
            {
                if (dto is null)
                {
                    return BadRequest();
                }

                var result = await _contentService.SaveFeatureAsync(dto);
                if (result is null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while saving the requested object.");
            }
        }

        /// <summary>
        /// Creates a new feature item, or updates an existing one.
        /// </summary>
        /// <param name="dto">The feature item to save.</param>
        [HttpPost]
        [Route("SaveFeatureItem")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<FeatureItem>), 200)]
        public async Task<ActionResult<FeatureItem>> SaveAsync(FeatureItem dto)
        {
            try
            {
                if (dto is null)
                {
                    return BadRequest();
                }

                var result = await _contentService.SaveFeatureItemAsync(dto);
                if (result is null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while saving the requested object.", statusCode: 500);
            }
        }

        /// <summary>
        /// Gets the latest release and pre-release tags, downloads xmldoc assets, and processes them.
        /// </summary>
        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult> UpdateXmlDocContentAsync()
        {
            try
            {
                await _xmlDocService.SynchronizeAsync();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while updating xmldoc content.", statusCode: 500);
            }
        }
    }
}
