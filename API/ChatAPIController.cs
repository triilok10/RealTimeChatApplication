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
                    cmd.Parameters.AddWithValue("@ChatUserID", DBNull.Value);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ChatMessage obj = new ChatMessage
                            {
                                SearchConnection = reader["UserName"].ToString(),
                                ChatMessageID = Convert.ToInt32(reader["ChatUserID"]),
                                ProfilePictureURL = Convert.ToString(reader["ProfilePictureURL"])
                            };
                            lstMessage.Add(obj);
                        }
                    }
                }
                return Ok(lstMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while searching for connections.");
            }
        }

        [HttpGet]
        public IActionResult GetProfile(string Id = "")
        {
            string message = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_ChatMessage", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 2);
                    cmd.Parameters.AddWithValue("@ChatUserID", Id);
                    cmd.Parameters.AddWithValue("UserName", DBNull.Value);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ChatMessage obj = new ChatMessage
                            {
                                ChatMessageID = Convert.ToInt32(rdr["ChatUserID"]),
                                Email = Convert.ToString(rdr["Email"]),
                                UserName = Convert.ToString(rdr["UserName"]),
                                ProfilePictureURL = Convert.ToString(rdr["ProfilePictureURL"])
                            };
                            return Ok(obj);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return StatusCode(500, "An error occurred while searching for connections.");
            }





            return Ok();
        }

    }
}
