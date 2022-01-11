using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Rubberduck.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        /// <summary>
        /// Login with GitHub.
        /// </summary>
        [HttpGet]
        [Route("Login")]
        public ActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl });
        }
    }
}
