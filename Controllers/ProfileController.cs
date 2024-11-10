using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RealTimeChatApplication.AppCode;
using RealTimeChatApplication.Hubs;
using RealTimeChatApplication.Models;

namespace RealTimeChatApplication.Controllers
{
    public class ProfileController : Controller
    {
        private readonly HttpClient _httpClient;
        IHttpContextAccessor _httpContextAccessor;
        private readonly dynamic baseUrl;
        private readonly IChatHub _signalRHub;

        private readonly ISessionService _sessionService;
        public ProfileController(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ISessionService sessionService, IChatHub signalRHub)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _sessionService = sessionService;
            _signalRHub = signalRHub;
            var request = _httpContextAccessor.HttpContext.Request;
            baseUrl = $"{request.Scheme}://{request.Host.Value}/"; _httpClient.BaseAddress = new Uri(baseUrl);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var Id = _sessionService.GetInt32("UserID");
            var UserName = _sessionService.GetString("UserName");
            ViewBag.UserName = UserName;

            try
            {
                string url = baseUrl + $"api/ProfileAPI/UpdateProfile?UpdateProfileID={Id}";
                HttpResponseMessage res = await _httpClient.GetAsync(url);
                if (res.IsSuccessStatusCode)
                {
                    dynamic resBody = await res.Content.ReadAsStringAsync();
                    ChatUser obj = JsonConvert.DeserializeObject<ChatUser>(resBody);
                    if (obj.Success == false)
                    {
                        return RedirectToAction("", "");
                    }
                    return View(obj);
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }
    }
}
