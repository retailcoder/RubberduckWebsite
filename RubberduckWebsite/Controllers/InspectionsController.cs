using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rubberduck.Client.Abstract;
using RubberduckWebsite.Controllers.Abstract;
using RubberduckWebsite.Models;

namespace RubberduckWebsite.Controllers
{

    [Route("{controller}")]
    public class InspectionsController : PublicApiClientController<InspectionDetailsViewModel>
    {
        public InspectionsController(ILogger<InspectionsController> logger, IPublicApiClient apiClient)
            : base(logger, apiClient) { }

        /// <summary>
        /// Gets the details page for the specified inspection.
        /// </summary>
        [Route("/{name}")]
        public async Task<IActionResult> Details([FromRoute]string name)
        {
            var item = await ApiClient.GetFeatureItemAsync(name);
            if (item is null)
            {
                return NotFound();
            }

            var vm = new InspectionDetailsViewModel(item);
            return View(vm);
        }

        /// <summary>
        /// Redirects to the Features controller to get the details for the "Inspections" feature (which lists a summary of all sub-features, including "CodeInspections").
        /// </summary>
        /// <remarks>
        /// Route "/Inspections" must be supported for v2.4.x Rubberduck VBIDE add-in clients.
        /// </remarks>
        public override async Task<IActionResult> Index() =>
            await Task.Run(() => RedirectToActionPermanentPreserveMethod("Details", "Features", new { name = "Inspections" }));

        protected override Task<InspectionDetailsViewModel> GetViewModelAsync() =>
            throw new NotSupportedException();
        
    }
}
