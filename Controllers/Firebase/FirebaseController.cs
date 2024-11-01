using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RealTimeChatApplication.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;

namespace RealTimeChatApplication.Controllers.Firebase
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : ControllerBase
    {

        private readonly HttpClient _httpClient;

        public FirebaseController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> SendNotification([FromBody] UserPendingNotification notification)
        {
            try
            {
                if (notification == null || string.IsNullOrEmpty(notification.FCMToken))
                {
                    return BadRequest("Invalid notification data.");
                }

                string firebaseUrl = "https://fcm.googleapis.com/fcm/send";


                string serverKey = "YOUR_SERVER_KEY";

                var fcmMessage = new
                {
                    to = notification.FCMToken,
                    notification = new
                    {
                        title = "New Connection Request",
                        body = notification.Message
                    },
                    data = new
                    {
                        userId = notification.LoginUserID,
                        message = notification.Message
                    }
                };
                string fcmJson = JsonConvert.SerializeObject(fcmMessage);
                StringContent content = new StringContent(fcmJson, Encoding.UTF8, "application/json");

              
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", "=" + serverKey);

                
                HttpResponseMessage response = await _httpClient.PostAsync(firebaseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok("Notification sent successfully.");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Failed to send notification. Error: {error}");
                }

            }
            catch (Exception ex) { }
            return Ok();
        }


    }
}
