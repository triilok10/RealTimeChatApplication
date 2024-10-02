using Microsoft.AspNetCore.Mvc;

namespace RealTimeChatApplication.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region "Login"
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        #endregion
    }
}
