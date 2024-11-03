using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using RealTimeChatApplication.AppCode;
using RealTimeChatApplication.Models;
using System.Text;

namespace RealTimeChatApplication.Controllers
{
    [ServiceFilter(typeof(SessionAdmin))]
    public class ConnectionsController : Controller
    {

        private readonly HttpClient _httpClient;
        IHttpContextAccessor _httpContextAccessor;
        private readonly dynamic baseUrl;

        private readonly ISessionService _sessionService;
        public ConnectionsController(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ISessionService sessionService)
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

        [HttpGet]
        public async Task<IActionResult> ConnectionList()
        {
            return PartialView();
        }


        [HttpGet]
        public async Task<JsonResult> GetPendingRequests()
        {

            var UserId = _sessionService.GetInt32("UserID");

            string url = baseUrl + "api/ConnectionRequest/GetPendingRequests";

            ChatMessage obj = new ChatMessage();
            obj.ChatMessageID = UserId;
            string JsonData = JsonConvert.SerializeObject(obj);
            StringContent content = new StringContent(JsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage res = await _httpClient.PostAsync(url, content);


            return Json(new { });
        }

        [HttpGet]
        public async Task<JsonResult> GetConnections()
        {
            var UserId = _sessionService.GetInt32("UserID");

            string url = baseUrl + "api/ConnectionRequest/GetConnections";

            ChatMessage obj = new ChatMessage();
            obj.ChatMessageID = UserId;
            string JsonData = JsonConvert.SerializeObject(obj);
            StringContent content = new StringContent(JsonData, Encoding.UTF8, "application/json");


            HttpResponseMessage res = await _httpClient.PostAsync(url, content);
            return Json(new { });
        }
    }
}
