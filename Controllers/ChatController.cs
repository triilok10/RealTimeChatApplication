using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using NuGet.Common;
using RealTimeChatApplication.AppCode;
using RealTimeChatApplication.Models;
using System.Drawing;
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
        public async Task<IActionResult> ChatBox()
        {

            var actionPath = HttpContext.Request.Path;
            _sessionService.SetString("ActionPath", actionPath);
            string message = "";
            try
            {
                var UserId = _sessionService.GetInt32("UserID");

                string url = baseUrl + "api/ChatAPI/LoadChatHistory";
                ChatMessage pChatMessage = new ChatMessage();

                pChatMessage.ChatMessageID = UserId;
                string Json = JsonConvert.SerializeObject(pChatMessage);
                StringContent content = new StringContent(Json, Encoding.UTF8, "application/json");

                HttpResponseMessage res = await _httpClient.PostAsync(url, content);

                if (res.IsSuccessStatusCode)
                {
                    dynamic resBody = await res.Content.ReadAsStringAsync();
                    List<ChatMessage> lstChatMessage = JsonConvert.DeserializeObject<List<ChatMessage>>(resBody);
                    ViewBag.lstChatMessage = lstChatMessage;
                    return View();
                }
                else
                {
                    return RedirectToAction("", "");
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return RedirectToAction("", "");
            }
        }


        [HttpGet]
        public async Task<IActionResult> ChatMessage(int Id = 0)
        {
            string message = "";
            bool response = false;
            var actionPath = HttpContext.Request.Path;
            _sessionService.SetString("ActionPath", actionPath);

            try
            {
                if (Id == 0)
                {
                    TempData["errorMessage"] = "Please select the User to Send Message";
                    return RedirectToAction("ChatBox", "Chat");
                }

                string url = baseUrl + $"api/ChatAPI/GetProfile";
                string fullUrl = url + $"?Id={Id}";
                HttpResponseMessage res = await _httpClient.GetAsync(fullUrl);
                if (res.IsSuccessStatusCode)
                {
                    dynamic resBody = await res.Content.ReadAsStringAsync();
                    ChatMessage obj = JsonConvert.DeserializeObject<ChatMessage>(resBody);
                    return View(obj);
                }
                else
                {
                    message = "Error in the API Call or to Fetch the Data";
                }
            }
            catch (Exception ex)
            {
                response = false;
                message = ex.Message;
            }
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
                    TempData["errorMessage"] = "Invalid, Please enter the UserName";
                    TempData.Keep("errorMessage");
                    return RedirectToAction("ChatBox");
                }
                string url = baseUrl + "api/ChatAPI/SearchConnections";
                string Json = JsonConvert.SerializeObject(pChatMessage);
                StringContent content = new StringContent(Json, Encoding.UTF8, "application/json");
                HttpResponseMessage res = await _httpClient.PostAsync(url, content);
                if (res.IsSuccessStatusCode)
                {
                    dynamic resBody = await res.Content.ReadAsStringAsync();
                    dynamic resData = JsonConvert.DeserializeObject(resBody);
                    if (resData != null)
                    {
                        List<ChatMessage> lstChat = JsonConvert.DeserializeObject<List<ChatMessage>>(resBody);
                        return Ok(new { lstChat });
                    }
                    else
                    {
                        return Ok(new { message = "No data available" });
                    }
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
