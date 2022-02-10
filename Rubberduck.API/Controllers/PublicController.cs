using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model;
using Rubberduck.Model.Entities;
using RubberduckServices.Abstract;

namespace Rubberduck.API.Controllers
{
    /// <summary>
    /// Exposes endpoints providing the website's dynamic content.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PublicController : ControllerBase
    {
        private readonly ILogger<PublicController> _logger;
        private readonly IContentService _content;
        private readonly IIndenterService _indenterService;

        /// <summary>
        /// Creates a controller that exposes endpoints providing the website's dynamic content.
        /// </summary>
        public PublicController(ILogger<PublicController> logger, IContentService content, IIndenterService indenterService)
        {
            _logger = logger;
            _content = content;
            _indenterService = indenterService;
        }

        /// <summary>
        /// Gets all top-level features, along with their sub-features and feature items.
        /// </summary>
        [HttpGet]
        [Route("Features")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Feature[]), 200)]
        public async Task<ActionResult<Feature[]>> GetFeaturesAsync()
        {
            try
            {
                var features = await _content.GetFeaturesAsync();
                return Ok(features);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while retrieving the requested objects.", statusCode: 500);
            }
        }

        /// <summary>
        /// Gets a feature or sub-feature along with its sub-features and feature items.
        /// </summary>
        [HttpGet]
        [Route("Features/{name}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Feature), 200)]
        public async Task<ActionResult<Feature>> GetFeatureAsync([FromRoute]string name)
        {
            try
            {
                var feature = await _content.GetFeatureAsync(name);
                if (feature is null)
                {
                    return NotFound();
                }
                return Ok(feature);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while retrieving the requested object.", statusCode: 500);
            }
        }

        /// <summary>
        /// Gets the specified feature item, including its examples and their respective modules.
        /// </summary>
        /// <param name="name">The unique name of the feature item to get.</param>
        [HttpGet]
        [Route("FeatureItem/{name}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(FeatureItem), 200)]
        public async Task<ActionResult<FeatureItem>> GetFeatureItem([FromRoute]string name)
        {
            try
            {
                var item = await _content.GetFeatureItemAsync(name);
                if (item is null)
                {
                    return NotFound();
                }
                return Ok(item);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogWarning(e, "A NotFound (404) result will be returned.");
                return NotFound();
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
        [ProducesResponseType(typeof(Tag[]), 200)]
        public async Task<ActionResult<Tag[]>> GetLatestTagsAsync()
        {
            try
            {
                var main = await _content.GetMainTagAsync();
                var next = await _content.GetNextTagAsync();
                return Ok(new[] { main, next }.Where(e => e != null));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while retrieving the latest tags.", statusCode: 500);
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
        public async Task<ActionResult<string[]>> IndentAsync([FromBody]IndenterViewModel viewModel)
        {
            try
            {
                var result = await _indenterService.IndentAsync(viewModel);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while indenting the provided code.", statusCode: 500);
            }
        }

        /// <summary>
        /// Gets a default indenter settings view model
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DefaultIndenterSettings")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string[]), 200)]
        public async Task<ActionResult<IndenterViewModel>> GetDefaultIndenterSettingsViewModelAsync()
        {
            try
            {
                var result = new IndenterViewModel
                {
                    IndenterVersion = _indenterService.IndenterVersion(),
                    Code = "Option Explicit\n\nPublic Sub DoSomething()\nEnd Sub\nPublic Sub DoSomethingElse()\n\nEnd Sub\n",
                    AlignCommentsWithCode = true,
                    EmptyLineHandlingMethod = Model.Abstract.IndenterEmptyLineHandling.Indent,
                    ForceCompilerDirectivesInColumn1 = true,
                    GroupRelatedProperties = false,
                    IndentSpaces = 4,
                    IndentCase = true,
                    IndentEntireProcedureBody = true,
                    IndentEnumTypeAsProcedure = true,
                    VerticallySpaceProcedures = true,
                    LinesBetweenProcedures = 1,
                    IndentFirstCommentBlock = true,
                    IndentFirstDeclarationBlock = true,
                    EndOfLineCommentStyle = Model.Abstract.IndenterEndOfLineCommentStyle.SameGap,
                };
                return await Task.FromResult(Ok(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while creating a viewmodel for indenter settings.", statusCode: 500);
            }
        }

        /// <summary>
        /// Searches features and feature items for filtered content.
        /// </summary>
        [HttpPost]
        [Route("Search")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SearchResultsViewModel), 200)]
        public async Task<ActionResult<SearchResultsViewModel>> SearchAsync(SearchViewModel search)
        {
            try
            {
                var result = await _content.SearchAsync(search.Query);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while searching for content.", statusCode: 500);
            }
        }
    }
}
