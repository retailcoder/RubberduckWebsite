using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rubberduck.API.Services.Abstract;
using Rubberduck.ContentServices;
using RubberduckServices.Abstract;

namespace Rubberduck.API.Controllers.Authenticated
{
    /// <summary>
    /// Exposes endpoints providing an interface to download xmldoc assets.
    /// </summary>
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class XmlDocAssetsController : ControllerBase
    {
        private readonly ILogger<XmlDocAssetsController> _logger;
        private readonly ISyntaxHighlighterService _syntaxHighlighter;
        private readonly IXmlDocServices _service;
        private readonly RubberduckDbContext _context;


        /// <summary>
        /// Creates a controller that exposes endpoints providing an interface to download xmldoc assets.
        /// </summary>
        public XmlDocAssetsController(ILogger<XmlDocAssetsController> logger, IXmlDocServices service, ISyntaxHighlighterService syntaxHighlighter, RubberduckDbContext context)
        {
            _logger = logger;
            _service = service;
            _syntaxHighlighter = syntaxHighlighter;
            _context = context;
        }

        /// <summary>
        /// Gets the latest release and pre-release tags, downloads xmldoc assets, and processes them.
        /// </summary>
        /// <returns>If successful, the number of milliseconds elapsed while processing the request.</returns>
        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult> UpdateXmlDocContentAsync()
        {
            try
            {
                var sw = Stopwatch.StartNew();
                await _service.SynchronizeAsync();
                sw.Stop();
                
                var result = $"{nameof(UpdateXmlDocContentAsync)} completed in {sw.ElapsedMilliseconds:N} milliseconds.";

                _logger.LogInformation(result);
                await _context.SaveChangesAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while updating xmldoc content.", statusCode: 500);
            }
        }

        /// <summary>
        /// Parses and formats the provided code string.
        /// </summary>
        [HttpPost]
        [Route("FormatCodeString")]
        public async Task<ActionResult<string>> GetFormattedCodeModuleAsync([FromBody]string content)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                var result = await _syntaxHighlighter.FormatAsync(content);
                sw.Stop();

                _logger.LogInformation($"{nameof(GetFormattedCodeModuleAsync)} completed in {sw.ElapsedMilliseconds:N} milliseconds.");
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while updating xmldoc content.", statusCode: 500);
            }
        }
    }
}
