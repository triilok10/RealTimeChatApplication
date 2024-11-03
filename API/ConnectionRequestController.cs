using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatApplication.Models;

namespace RealTimeChatApplication.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConnectionRequestController : ControllerBase
    {

        [HttpPost]
        public IActionResult GetConnections(ChatMessage pChatMessage)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult GetPendingRequests(ChatMessage pChatMessage)
        {
            return Ok();
        }


    }
}
