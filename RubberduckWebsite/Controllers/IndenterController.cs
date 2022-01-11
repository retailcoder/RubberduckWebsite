using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rubberduck.Client.Abstract;
using Rubberduck.Model;
using RubberduckWebsite.Controllers.Abstract;

namespace RubberduckWebsite.Controllers
{
    [Route("{controller}")]
    public class IndenterController : PublicApiClientController<object>
    {
        public IndenterController(ILogger<IndenterController> logger, IPublicApiClient apiClient)
            : base(logger, apiClient) { }

        public override async Task<IActionResult> Index()
        {
            var vm = new IndenterViewModel();
            return View(vm);
        }

        [HttpPost]
        [Route("/Indent")]
        public async Task<IActionResult> IndentAsync([FromBody]IndenterViewModel model)
        {
            var result = await ApiClient.GetIndentedCodeAsync(model);
            return Ok(result);
        }

        protected override Task<object> GetViewModelAsync() =>
            throw new NotSupportedException();
    }
}
