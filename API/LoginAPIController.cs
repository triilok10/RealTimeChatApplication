using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatApplication.Models;

namespace RealTimeChatApplication.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginAPIController : ControllerBase
    {
        [HttpPost]
        public IActionResult Register([FromBody] ChatUser pChatUser)
        {
            return Ok();
        }
    }
}
