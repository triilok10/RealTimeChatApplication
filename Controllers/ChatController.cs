using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RealTimeChatApplication.AppCode;

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
        #endregion
    }
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
