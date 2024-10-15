using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatApplication.Models;
using System.Data;
using System.Data.SqlClient;

namespace RealTimeChatApplication.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatAPIController : ControllerBase
    {
        private readonly string _connectionString;

        public ChatAPIController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CustomConnection");
        }


        [HttpPost]
        public IActionResult SearchConnections([FromBody] ChatMessage pChatMessage)
        {
            List<ChatMessage> lstMessage = new List<ChatMessage>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_ChatMessage", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 1);
                    cmd.Parameters.AddWithValue("@UserName", pChatMessage.SearchConnection);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ChatMessage obj = new ChatMessage
                            {
                                SearchConnection = reader["UserName"].ToString(),
                                ChatMessageID = Convert.ToInt32(reader["ChatUser"])
                            };
                            lstMessage.Add(obj);
                        }
                    }
                }
                return Ok(new { lstMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while searching for connections.");
            }
        }

    }
}
