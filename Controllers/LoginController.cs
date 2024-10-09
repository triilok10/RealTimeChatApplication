using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RealTimeChatApplication.AppCode;
using RealTimeChatApplication.Models;
using System.Reflection.Metadata.Ecma335;
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
        public IActionResult Register(ChatUser pChatUser)
        {
            return View(pChatUser);
        }
        [HttpPost]
        public async Task<IActionResult> Register(ChatUser pChatUser, IFormFile ProfilePictureURL, string ProfilePictureURLCamera)
        {
            if (!string.IsNullOrEmpty(ProfilePictureURLCamera))
            {
                string base64Data = ProfilePictureURLCamera.Replace("data:image/jpeg;base64,", "");

                byte[] imageBytes = Convert.FromBase64String(base64Data);

                string filePath = Path.Combine("wwwroot", "images", "uploads");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string fileName = "profile_picture.jpg";
                string directoryPath = Path.Combine(filePath, fileName);

                System.IO.File.WriteAllBytes(directoryPath, imageBytes);
            }

            if (ProfilePictureURL != null)
            {
                string FileName = Path.GetFileName(ProfilePictureURL.FileName);
                string FilePath = Path.Combine("wwwroot", "images", "Uploads", FileName);

                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }
                using (var stream = new FileStream(FilePath, FileMode.Create))
                {
                    await ProfilePictureURL.CopyToAsync(stream);
                }
                pChatUser.HdnProfilePicture = FileName;

            }


            string url = baseUrl + "api/LoginAPI/Register";

            string Json = JsonConvert.SerializeObject(pChatUser);
            StringContent content = new StringContent(Json, Encoding.UTF8, "application/json");

            HttpResponseMessage res = await _httpClient.PostAsync(url, content);
            if (res.IsSuccessStatusCode)
            {

                dynamic resData = await res.Content.ReadAsStringAsync();
                dynamic resBody = JsonConvert.DeserializeObject(resData);
                if (resBody.response == true)
                {
                    string Message = resBody.message;
                    TempData["successMessage"] = Message;
                    TempData.Keep("successMessage");
                    return RedirectToAction("ChatBox", "Chat");
                }
                else
                {
                    string Message = resBody.message;
                    TempData["errorMessage"] = Message;
                    return RedirectToAction("Register", pChatUser);
                }

            }
            return RedirectToAction("Register", "Login");
        }


        [HttpGet]
        public PartialViewResult _partialTermsCondition(string termsValueCheck = "")
        {

            ViewBag.HdnTermsCondition = termsValueCheck; ;
            return PartialView();
        }

        [HttpPost]
        public JsonResult TermsCondition(bool boolTerms)
        {
            if (boolTerms == true)
            {
                ChatUser obj = new ChatUser();
                obj.TermsCondition = boolTerms;
                return Json(new { boolTerms });
            }
            return Json(new { boolTerms });
        }

        #endregion
    }
}
