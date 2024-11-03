using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using RealTimeChatApplication.AppCode;
using RealTimeChatApplication.Models;
using System.Collections.Generic;
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
            try
            {

                var UserId = _sessionService.GetInt32("UserID");

                string url = baseUrl + "api/ConnectionRequest/GetPendingRequests";

                ChatMessage obj = new ChatMessage();
                obj.ChatMessageID = UserId;
                string JsonData = JsonConvert.SerializeObject(obj);
                StringContent content = new StringContent(JsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage res = await _httpClient.PostAsync(url, content);
                if (res.IsSuccessStatusCode)
                {
                    dynamic resData = await res.Content.ReadAsStringAsync();
                    List<ChatMessage> lst = JsonConvert.DeserializeObject<List<ChatMessage>>(resData);
                    return Json(new { type = "pendingRequests", lst });
                }
                else
                {
                    List<ChatMessage> lst = new List<ChatMessage>();
                    lst = null;
                    return Json(new { lst });
                }
            }
            catch (Exception ex)
            {
                return Json(new { });
            }
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
            if (res.IsSuccessStatusCode)
            {
                dynamic resData = await res.Content.ReadAsStringAsync();
                List<ChatMessage> lst = JsonConvert.DeserializeObject<List<ChatMessage>>(resData);
                return Json(new { type = "connections", lst });
            }
            else
            {
                List<ChatMessage> lst = new List<ChatMessage>();
                lst = null;
                return Json(new { lst });
            }
        }


        [HttpGet]
        public async Task<JsonResult> AcceptRequest(string id = "")
        {
            string message = "";
            bool response = false;
            var UserId = _sessionService.GetInt32("UserID");
            string url = baseUrl + $"api/ConnectionRequest/AcceptRequest?AcceptID={UserId}&RequestID={id}";

            HttpResponseMessage res = await _httpClient.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                dynamic resBody = await res.Content.ReadAsStringAsync();
                dynamic resData = JsonConvert.DeserializeObject<dynamic>(resBody);
                message = resData.msg;
                response = resData.res;

            }

            return Json(new { message, response });
        }


        [HttpGet]
        public async Task<JsonResult> RejectRequest(string id = "")
        {
            string message = "";
            bool response = false;
            var UserId = _sessionService.GetInt32("UserID");
            string url = baseUrl + $"api/ConnectionRequest/RejectRequest?AcceptID={UserId}&RequestID={id}";

            HttpResponseMessage res = await _httpClient.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                dynamic resBody = await res.Content.ReadAsStringAsync();
                dynamic resData = JsonConvert.DeserializeObject<dynamic>(resBody);
                message = resData.msg;
                response = resData.res;
            }
            return Json(new { message, response });
        }
    }
}
