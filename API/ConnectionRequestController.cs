using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatApplication.Models;
using System.Data.SqlClient;

namespace RealTimeChatApplication.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConnectionRequestController : ControllerBase
    {
        private readonly string _connectionString;

        public ConnectionRequestController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CustomConnection");
        }


        [HttpPost]
        public IActionResult GetPendingRequests(ChatMessage pChatMessage)
        {
            bool res = false;
            string msg = "";
            List<ChatMessage> lstPendingRequest = new List<ChatMessage>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_ConnectionRequest", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", 1);
                    cmd.Parameters.AddWithValue("@UserID", pChatMessage.ChatMessageID);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ChatMessage chatMessage = new ChatMessage
                            {
                                ChatUserID = Convert.ToInt32(rdr["ChatUserID"]),
                                FullName = Convert.ToString(rdr["FullName"]),
                                ProfilePictureURL = Convert.ToString(rdr["ProfilePictureURL"]),
                            };
                            lstPendingRequest.Add(chatMessage);
                        }
                    }

                    return Ok(lstPendingRequest);
                }

            }
            catch (Exception ex)
            {
                res = false;
                msg = ex.Message;
                return Ok(new { res = res, msg = msg });
            }


        }

        [HttpPost]
        public IActionResult GetConnections(ChatMessage pChatMessage)
        {
            bool res = false;
            string msg = "";
            List<ChatMessage> lstGetConnections = new List<ChatMessage>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_ConnectionRequest", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", 2);
                    cmd.Parameters.AddWithValue("@UserID", pChatMessage.ChatMessageID);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ChatMessage chatMessage = new ChatMessage
                            {
                                ChatUserID = Convert.ToInt32(rdr["ChatUserID"]),
                                FullName = Convert.ToString(rdr["FullName"]),
                                ProfilePictureURL = Convert.ToString(rdr["ProfilePictureURL"]),
                            };
                            lstGetConnections.Add(chatMessage);
                        }
                    }

                    return Ok(lstGetConnections);
                }
            }
            catch (Exception ex)
            {
                res = false;
                msg = ex.Message;
                return Ok(new { res = res, msg = msg });
            }
        }
    }
}
