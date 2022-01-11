using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rubberduck.API.Services.Abstract;
using Rubberduck.ContentServices.Service.Abstract;
using Rubberduck.Model;
using RubberduckServices.Abstract;

namespace Rubberduck.API.Controllers
{
    /// <summary>
    /// A controller that exposes an endpoint that runs quick checks periodically.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private static readonly string _checkStartMessage = "health check started";
        private static readonly string _successMessage = "health check succeeded";
        private static readonly string _failureMessage = "health check failed";

        private readonly ILogger<HealthController> _logger;
        private readonly IContentService _content;
        private readonly IIndenterService _indenter;
        private readonly IGitHubDataServices _gitHub;

        /// <summary>
        /// Creates a controller that exposes an endpoint that runs quick checks periodically.
        /// </summary>
        public HealthController(ILogger<HealthController> logger,
            IContentService content,
            IIndenterService indenter,
            IGitHubDataServices gitHub)
        {
            _logger = logger;
            _content = content;
            _indenter = indenter;
            _gitHub = gitHub;
        }

        /// <summary>
        /// Runs a number of internal quick checks to ensure base services are operational.
        /// </summary>
        [HttpGet]
        [Route("check")]
        public async Task<ActionResult> CheckAsync()
        {
            _logger.LogTrace(_checkStartMessage);

            var results = await Task.WhenAll(
                CheckDbReaderAsync(),
                CheckIndenterAsync(),
                CheckGitHubAsync()
                );

            if (results.All(result => result is OkResult))
            {
                _logger.LogInformation(_successMessage);
                return Ok();
            }

            return Problems(results);
        }

        private ActionResult Problems(ActionResult[] results)
        {
            var problems = string.Join("\n\n", results
                .Where(result => result is not OkResult).Cast<ObjectResult>()
                .Select(result => result.Value).Cast<ProblemDetails>()
                .Select(problem => $"{problem.Title}\n{problem.Detail})"));

            return Problem(problems);
        }

        private async Task<ActionResult> CheckDbReaderAsync()
        {
            try
            {
                _ = await _content.GetFeaturesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                var message = $"{nameof(CheckDbReaderAsync)}: {_failureMessage}";
                _logger.LogWarning(e, message);
                return Problem(e.ToString(), title: message);
            }
        }

        private async Task<ActionResult> CheckIndenterAsync()
        {
            try
            {
                var vm = new IndenterViewModel { Code = "Option Explicit" };
                _ = await _indenter.IndentAsync(vm);
                return Ok();
            }
            catch (Exception e)
            {
                var message = $"{nameof(CheckIndenterAsync)}: {_failureMessage}";
                _logger.LogWarning(e, message);
                return Problem(e.ToString(), title: message);
            }
        }

        private async Task<ActionResult> CheckGitHubAsync()
        {
            try
            {
                var config = await _gitHub.GetCodeAnalysisDefaultsConfig();                
                return Ok();
            }
            catch (Exception e)
            {
                var message = $"{nameof(CheckGitHubAsync)}: {_failureMessage}";
                _logger.LogWarning(e, message);
                return Problem(e.ToString(), title: message);
            }
        }
    }
}
