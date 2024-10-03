using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RealTimeChatApplication.AppCode;
using RealTimeChatApplication.Models;
using System.Text;

namespace RealTimeChatApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;
        IHttpContextAccessor _httpContextAccessor;
        private readonly dynamic baseUrl;

        private readonly ISessionService _sessionService;
        public LoginController(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ISessionService sessionService)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _sessionService = sessionService;
            var request = _httpContextAccessor.HttpContext.Request;
            baseUrl = $"{request.Scheme}://{request.Host.Value}/"; _httpClient.BaseAddress = new Uri(baseUrl);
        }

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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(ChatUser pChatUser, IFormFile ProfilePictureURL, IFormFile ProfilePictureURLCamera)
        {


            string url = baseUrl + "api/LoginAPI/Register";

            string Json = JsonConvert.SerializeObject(pChatUser);
            StringContent content = new StringContent(Json, Encoding.UTF8, "application/json");

            HttpResponseMessage res = await _httpClient.PostAsync(url, content);
            if (res.IsSuccessStatusCode)
            {

            }
            return RedirectToAction("", "");
        }


        #endregion
    }
}
