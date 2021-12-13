using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rubberduck.API.Services.Abstract;
using Rubberduck.Model.Internal;

namespace Rubberduck.API.Controllers.Authenticated
{
    /// <summary>
    /// Exposes endpoints providing an interface to manipulate the website's dynamic content.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ContentController : ControllerBase
    {
        private readonly ILogger<ContentController> _logger;
        private readonly IContentServices _service;

        /// <summary>
        /// Creates a controller that exposes endpoints providing an interface to manipulate the website's dynamic content.
        /// </summary>
        public ContentController(ILogger<ContentController> logger, IContentServices service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Creates a new feature (or sub-feature), or updates an existing one.
        /// </summary>
        /// <param name="dto">The feature (or sub-feature) to save.</param>
        [HttpPost]
        [Route("SaveFeature")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.DTO.Feature>), 200)]
        public async Task<ActionResult<Model.DTO.Feature>> SaveAsync(Model.DTO.Feature dto)
        {
            try
            {
                if (dto is null)
                {
                    return BadRequest();
                }

                var result = await _service.SaveAsync(dto);
                if (result is null)
                {
                    return NotFound();
                }

                return Ok(Feature.ToDTO(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while saving the requested object.");
            }
        }

        /*
        /// <summary>
        /// Deletes the specified feature.
        /// </summary>
        /// <param name="dto">The feature to delete.</param>
        [HttpPost]
        [Route("DeleteFeature")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        public async Task<ActionResult> DeleteAsync(Model.DTO.Feature dto)
        {
            try
            {
                if (dto is null || dto.Id == 0)
                {
                    return BadRequest();
                }

                var entity = Feature.FromDTO(dto);
                var existing = await _featuresReader.GetByIdAsync(entity.Id);
                if (existing is null)
                {
                    return NotFound();
                }
                else
                {
                    await _featuresWriter.DeleteAsync(entity);
                    return Ok();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while deleting the requested object.", statusCode: 500);
            }
        }
        */

        /// <summary>
        /// Creates a new feature item, or updates an existing one.
        /// </summary>
        /// <param name="dto">The feature item to save.</param>
        [HttpPost]
        [Route("SaveFeatureItem")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.DTO.FeatureItem>), 200)]
        public async Task<ActionResult<Model.DTO.FeatureItem>> SaveAsync(Model.DTO.FeatureItem dto)
        {
            try
            {
                if (dto is null)
                {
                    return BadRequest();
                }

                var result = await _service.SaveAsync(dto);
                if (result is null)
                {
                    return NotFound();
                }

                return Ok(FeatureItem.ToDTO(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while saving the requested object.", statusCode: 500);
            }
        }

        /*
        /// <summary>
        /// Deletes the specified feature item.
        /// </summary>
        /// <param name="dto">The feature item to delete.</param>
        [HttpPost]
        [Route("DeleteFeatureItem")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        public async Task<ActionResult> DeleteAsync(Model.DTO.FeatureItem dto)
        {
            try
            {
                if (dto is null || dto.Id == 0)
                {
                    return BadRequest();
                }

                var entity = FeatureItem.FromDTO(dto);
                var existing = await _featureItemsReader.GetByIdAsync(entity.Id);
                if (existing is null)
                {
                    return NotFound();
                }
                else
                {
                    await _featureItemsWriter.DeleteAsync(entity);
                    return Ok();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while deleting the requested object.", statusCode: 500);
            }
        }
        */

        /// <summary>
        /// Creates a new example, or updates an existing one.
        /// </summary>
        /// <param name="dto">The example to save.</param>
        [HttpPost]
        [Route("SaveExample")]
        [ProducesResponseType(typeof(IEnumerable<Model.DTO.Example>), 200)]
        public async Task<ActionResult<Model.DTO.Example>> SaveAsync(Model.DTO.Example dto)
        {
            try
            {
                if (dto is null)
                {
                    return BadRequest();
                }

                var result = await _service.SaveAsync(dto);
                if (result is null)
                {
                    return NotFound();
                }

                return Ok(Example.ToDTO(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while saving the requested object.", statusCode: 500);
            }
        }

        /*
        /// <summary>
        /// Deletes the specified example.
        /// </summary>
        /// <param name="dto">The example to delete.</param>
        [HttpPost]
        [Route("DeleteExample")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        public async Task<ActionResult> DeleteAsync(Model.DTO.Example dto)
        {
            try
            {
                if (dto is null || dto.Id == 0)
                {
                    return BadRequest();
                }

                var entity = Example.FromDTO(dto);
                var existing = await _examplesReader.GetByIdAsync(entity.Id);
                if (existing is null)
                {
                    return NotFound();
                }
                else
                {
                    await _examplesWriter.DeleteAsync(entity);
                    return Ok();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while deleting the requested object.", statusCode: 500);
            }
        }
        */

        /// <summary>
        /// Creates a new example code module, or updates an existing one.
        /// </summary>
        /// <param name="dto">The example code module to save.</param>
        [HttpPost]
        [Route("SaveExampleModule")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.DTO.ExampleModule>), 200)]
        public async Task<ActionResult<Model.DTO.ExampleModule>> SaveAsync(Model.DTO.ExampleModule dto)
        {
            try
            {
                if (dto is null)
                {
                    return BadRequest();
                }

                var result = await _service.SaveAsync(dto);
                if (result is null)
                {
                    return NotFound();
                }

                return Ok(ExampleModule.ToDTO(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while saving the requested object.", statusCode: 500);
            }
        }

        /*
        /// <summary>
        /// Deletes the specified example module.
        /// </summary>
        /// <param name="dto">The example module to delete.</param>
        [HttpPost]
        [Route("DeleteExampleModule")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ActionResult), 200)]
        public async Task<ActionResult> DeleteAsync(Model.DTO.ExampleModule dto)
        {
            try
            {
                if (dto is null || dto.Id == 0)
                {
                    return BadRequest();
                }

                var entity = ExampleModule.FromDTO(dto);
                var existing = await _exampleModulesReader.GetByIdAsync(entity.Id);
                if (existing is null)
                {
                    return NotFound();
                }
                else
                {
                    await _exampleModulesWriter.DeleteAsync(entity);
                    return Ok();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while deleting the requested object.", statusCode: 500);
            }
        }
        */

        /// <summary>
        /// Creates or updates the specified tag.
        /// </summary>
        /// <param name="dto">The tag to save.</param>
        [HttpPost]
        [Route("SaveTag")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.DTO.Tag>), 200)]
        public async Task<ActionResult<Model.DTO.Tag>> SaveAsync(Model.DTO.Tag dto)
        {
            try
            {
                if (dto is null)
                {
                    return BadRequest();
                }

                var result = await _service.SaveAsync(dto);
                if (result is null)
                {
                    return NotFound();
                }

                return Ok(Tag.ToDTO(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while saving the requested object.");
            }
        }

        /// <summary>
        /// Creates or updates the specified tag asset.
        /// </summary>
        /// <param name="dto">The tag asset to save.</param>
        [HttpPost]
        [Route("SaveTagAsset")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Model.DTO.TagAsset>), 200)]
        public async Task<ActionResult<Model.DTO.TagAsset>> SaveAsync(Model.DTO.TagAsset dto)
        {
            try
            {
                if (dto is null)
                {
                    return BadRequest();
                }

                var result = await _service.SaveAsync(dto);
                if (result is null)
                {
                    return NotFound();
                }

                return Ok(TagAsset.ToDTO(result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while saving the requested object.");
            }
        }
    }
}
