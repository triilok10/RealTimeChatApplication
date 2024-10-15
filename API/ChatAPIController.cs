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

                    DataTable DT = new DataTable();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ChatMessage obj = new ChatMessage
                            {
                                SearchConnection = Convert.ToString(reader["UserName"]),
                                ChatMessageID = Convert.ToInt32(reader["ChatUser"])
                            };
                        };
                    };
                }

            }
            catch (Exception ex) { }
            return Ok();
        }
    }
}
