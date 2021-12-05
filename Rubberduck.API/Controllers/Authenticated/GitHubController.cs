using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rubberduck.API.DTO;
using Rubberduck.API.Services.Abstract;
using Rubberduck.Model.Entity;

namespace Rubberduck.API.Controllers.Authenticated
{
    /// <summary>
    /// Exposes endpoints providing an interface to retrieve content from GitHub.
    /// </summary>
    //[Authorize]
    [ApiController]
    [Route("[Controller]")]
    public class GitHubController : ControllerBase
    {
        private readonly ILogger<GitHubController> _logger;
        private readonly IGitHubDataServices _service;

        /// <summary>
        /// A controller that exposes endpoints providing an interface to retrieve content from GitHub.
        /// </summary>
        public GitHubController(ILogger<GitHubController> logger, IGitHubDataServices service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Gets metadata for every tag that ever was.
        /// </summary>
        [HttpGet]
        [Route("Tags")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.DTO.Tag>), 200)]
        public async Task<ActionResult<IEnumerable<Model.DTO.Tag>>> GetAllTagsAsync()
        {
            try
            {
                var results = await _service.GetAllTags();
                return Ok(results.Select(Tag.ToDTO));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while retrieving the specified tag.", statusCode: 500);
            }
        }

        /// <summary>
        /// Gets metadata about the current latest release tag.
        /// </summary>
        [HttpGet]
        [Route("Tags/Latest")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.DTO.Tag), 200)]
        public async Task<ActionResult<Model.DTO.Tag>> GetLatestReleaseAsync()
        {
            try
            {
                var result = await _service.GetTag(name:null);
                return Ok(Tag.ToDTO(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while retrieving the latest release tag.", statusCode: 500);
            }
        }

        /// <summary>
        /// Gets metadata about the specified tag.
        /// </summary>
        [HttpGet]
        [Route("Tag/{name}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Model.DTO.Tag), 200)]
        public async Task<ActionResult<Model.DTO.Tag>> GetTagAsync([FromRoute]string name)
        {
            try
            {
                if (string.Equals(name, "latest", StringComparison.InvariantCultureIgnoreCase))
                {
                    return await GetLatestReleaseAsync();
                }

                var result = await _service.GetTag(name);
                return Ok(Tag.ToDTO(result));
            }
            catch (Octokit.NotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while retrieving the specified tag.", statusCode: 500);
            }
        }

        /// <summary>
        /// Gets the current inspection default settings (inspection type and severity) configuration.
        /// </summary>
        [HttpGet]
        [Route("InspectionDefaults")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<InspectionDefaultConfig>), 200)]
        public async Task<ActionResult<IEnumerable<InspectionDefaultConfig>>> GetInspectionDefaultsAsync()
        {
            try
            {
                var result = await _service.GetCodeAnalysisDefaultsConfig();
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while retrieving the specified tag.", statusCode: 500);
            }
        }
    }
}
