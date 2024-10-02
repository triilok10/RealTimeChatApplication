using Microsoft.AspNetCore.Mvc;
using RealTimeChatApplication.Models;

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

        [HttpPost]
        public IActionResult Login(ChatUser pChatUser)
        {

            if (pChatUser.UserName == null)
            {
                ViewBag.ErrorMessage = "Please enter the UserName";
                return View();
            }
            if (pChatUser.Password == null)
            {
                ViewBag.ErrorMessage = "Please enter the Password";
                return View();
            }


            return RedirectToAction("Index", "Home");
        }

        [HttpGet, HttpPost]
        public IActionResult Register(ChatUser pChatUser)
        {
            if (pChatUser == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("", "");
            }
        }

        #endregion
    }
}
