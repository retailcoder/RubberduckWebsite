using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rubberduck.API.Services.Abstract;
using RubberduckServices.Abstract;

namespace Rubberduck.API.Controllers.Authenticated
{
    /// <summary>
    /// Exposes endpoints providing an interface to download xmldoc assets.
    /// </summary>
    //[Authorize]
    [ApiController]
    [Route("[Controller]")]
    public class XmlDocAssetsController : ControllerBase
    {
        private readonly ILogger<XmlDocAssetsController> _logger;
        private readonly ISyntaxHighlighterService _syntaxHighlighter;
        private readonly IXmlDocServices _service;

        /// <summary>
        /// Creates a controller that exposes endpoints providing an interface to download xmldoc assets.
        /// </summary>
        public XmlDocAssetsController(ILogger<XmlDocAssetsController> logger, IXmlDocServices service, ISyntaxHighlighterService syntaxHighlighter)
        {
            _logger = logger;
            _service = service;
            _syntaxHighlighter = syntaxHighlighter;
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
                var elapsedMilliseconds = await Task.Run(() => Stopwatch.StartNew())
                    .ContinueWith(async t =>
                    {
                        var sw = t.Result;
                        await _service.SynchronizeAsync();
                        sw.Stop();
                        return sw.ElapsedMilliseconds;
                    })
                    .ContinueWith(t =>
                    {
                        var ms = t.Result;
                        _logger.LogInformation($"{nameof(UpdateXmlDocContentAsync)} completed in {ms:N} milliseconds.");
                        return ms;
                    });
                return Ok(elapsedMilliseconds);
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
                var formattedCode = await Task.Run(() => Stopwatch.StartNew())
                    .ContinueWith(async t =>
                    {
                        var sw = await t;
                        var result = await _syntaxHighlighter.FormatAsync(content);
                        sw.Stop();
                        return (FormattedCode:result, sw.ElapsedMilliseconds);
                    })
                    .ContinueWith(async t =>
                    {
                        var (FormattedCode, ElapsedMilliseconds) = await t.Result;
                        _logger.LogInformation($"{nameof(GetFormattedCodeModuleAsync)} completed in {ElapsedMilliseconds:N} milliseconds.");
                        return FormattedCode;
                    });
                return Ok(formattedCode);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A Problem (500) result will be returned.");
                return Problem("An error has been logged while updating xmldoc content.", statusCode: 500);
            }
        }
    }
}
