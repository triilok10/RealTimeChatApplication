using Microsoft.AspNetCore.Mvc;
using RealTimeChatApplication.AppCode;

namespace RealTimeChatApplication.Controllers
{
    public class ChatController : Controller
    {
        private readonly HttpClient _httpClient;
        IHttpContextAccessor _httpContextAccessor;
        private readonly dynamic baseUrl;

        private readonly ISessionService _sessionService;
        public ChatController(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ISessionService sessionService)
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



        #region "ChatBox"
        [HttpGet]
        public IActionResult ChatBox()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ChatMessage()
        {
            return View();
        }
        #endregion
    }
}
