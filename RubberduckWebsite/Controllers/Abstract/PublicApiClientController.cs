using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Rubberduck.Client.Abstract;

namespace RubberduckWebsite.Controllers.Abstract
{
    /// <summary>
    /// A controller that gets its model using an <c>IPublicApiClient</c> service.
    /// </summary>
    public abstract class PublicApiClientController<TViewModel> : Controller
        where TViewModel : class
    {
        protected PublicApiClientController(ILogger logger, IPublicApiClient apiClient)
        {
            Logger = logger;
            ApiClient = apiClient;
        }

        /// <summary>
        /// Gets the logger for this instance.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Gets the API client service for this instance.
        /// </summary>
        protected IPublicApiClient ApiClient { get; }

        /// <summary>
        /// Creates and returns a ViewModel for this instance.
        /// </summary>
        protected abstract Task<TViewModel> GetViewModelAsync();

        public sealed override void OnActionExecuting(ActionExecutingContext context)
        {
            Logger.LogInformation($"Executing action {context.ActionDescriptor.DisplayName}. Request path: {Request.Path}");
        }

        public virtual async Task<IActionResult> Index()
        {
            var model = await GetViewModelAsync();
            return View(model);
        }
    }
}
