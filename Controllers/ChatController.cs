using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using RealTimeChatApplication.AppCode;
using RealTimeChatApplication.Models;
using System.Text;

namespace RealTimeChatApplication.Controllers
{
    [ServiceFilter(typeof(SessionAdmin))]
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
            var actionPath = HttpContext.Request.Path;
            _sessionService.SetString("ActionPath", actionPath);

            return View();

        }


        [HttpGet]
        public IActionResult ChatMessage()
        {
            var actionPath = HttpContext.Request.Path;
            _sessionService.SetString("ActionPath", actionPath);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchConnections(ChatMessage pChatMessage)
        {
            string message = "";
            try
            {
                if (pChatMessage.SearchConnection == null)
                {

                }

                string url = baseUrl + "";
                string Json = JsonConvert.SerializeObject(pChatMessage);
                StringContent content = new StringContent(Json, Encoding.UTF8, "application/json");
                HttpResponseMessage res = await _httpClient.PostAsync(url, content);
                if (res.IsSuccessStatusCode)
                {
                    dynamic resBody = await res.Content.ReadAsStringAsync();
                    string resData = JsonConvert.DeserializeObject(resBody);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;


            }
            return RedirectToAction("ChatBox", "Chat");

        }

    }
    #endregion
    public class SessionAdmin : ActionFilterAttribute
    {
        private readonly ISessionService _sessionService;

        public SessionAdmin(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            int? UserId = _sessionService.GetInt32("UserID");
            string Username = _sessionService.GetString("UserName");
            if (string.IsNullOrEmpty(Username) || UserId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Login", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
