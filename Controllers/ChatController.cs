using Microsoft.AspNetCore.Mvc;

namespace RealTimeChatApplication.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }



        #region "ChatBox"
        [HttpGet]
        public IActionResult ChatBox()
        {
            return View();
        }

        #endregion
    }
}
