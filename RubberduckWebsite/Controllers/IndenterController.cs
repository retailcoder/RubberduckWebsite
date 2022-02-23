using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rubberduck.Client;
using Rubberduck.Client.Abstract;
using Rubberduck.Model;
using RubberduckWebsite.Controllers.Abstract;

namespace RubberduckWebsite.Controllers
{
    [AllowAnonymous]
    [Route("{controller}")]
    public class IndenterController : PublicApiClientController<object>
    {
        public IndenterController(
            ILogger<IndenterController> logger, 
            IPublicApiClient apiClient)
            : base(logger, apiClient) { }

        [HttpPost]
        [Route("/Indent")]
        public async Task<IActionResult> IndentAsync([FromBody]IndenterViewModel model)
        {
            if (model is null)
            {
                return BadRequest();
            }

            var result = await ApiClient.GetIndentedCodeAsync(model);
            if (result is null)
            {
                return Problem("Indenter API failed to process the provided code and/or settings.");
            }

            return Ok(result);
        }

        protected override async Task<object> GetViewModelAsync() =>
            await ApiClient.GetDefaultIndenterSettings();

    }
}
