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


        #region "Load Chat History"
        [HttpPost]
        public IActionResult LoadChatHistory([FromBody] ChatMessage chatMessage)
        {
            string msg = "";
            bool res = false;
            try
            {
                List<ChatMessage> lstChatMessage = new List<ChatMessage>();
                using (SqlConnection con = new SqlConnection(_connectionString))
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_MessageRecord", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 2);
                    cmd.Parameters.AddWithValue("@ChatMessageID", chatMessage.ChatMessageID);                  

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ChatMessage obj = new ChatMessage
                            {
                                UserName = Convert.ToString(rdr["UserName"]),
                                FullName = Convert.ToString(rdr["FullName"]),
                                ChatReceiverID = Convert.ToInt32(rdr["ChatUserID"]),
                                ProfilePictureURL = Convert.ToString(rdr["ProfilePictureURL"])
                            };
                            lstChatMessage.Add(obj);
                        }
                    }
                    return Ok(lstChatMessage);
                }
            }
            catch (Exception ex)
            {
                res = false;
                msg = ex.Message;
                return StatusCode(500, "An error occurred while Fetching the Data.");
            }
        }
        #endregion

        #region "Chat History Record"
        [HttpPost]
        public IActionResult ChatHistoryRecord([FromBody] ChatMessage chatMessage)
        {
            string msg = "";
            bool res = false;
            try
            {
                List<ChatMessage> lstChatMessage = new List<ChatMessage>();
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_MessageRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 3);
                    cmd.Parameters.AddWithValue("@SenderID", chatMessage.ChatMessageID);      //User Currently Login to the System -- UserId
                    cmd.Parameters.AddWithValue("@ReceiverID", chatMessage.ChatReceiverID);   //User Who receive the Message


                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ChatMessage obj = new ChatMessage
                            {
                                ChatMessageID = Convert.ToInt32(rdr["SenderID"]),
                                ChatReceiverID = Convert.ToInt32(rdr["ReceiverID"]),
                                ChatMessageData = Convert.ToString(rdr["ChatMessage"]),
                                TimeStamp = Convert.ToDateTime(rdr["TimeStamp"])
                            };
                            lstChatMessage.Add(obj);
                        }
                    }
                }
                return Ok(lstChatMessage);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                res = false;
                return StatusCode(500, "An error occurred while Fetching the Data.");
            }
        }
        #endregion

        #region "User Connection Request"
        [HttpPost]
        public IActionResult RequestConnection(UserConnection userConnection)
        {
            bool res = false;
            string msg = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_MessageRecord", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 4);
                    cmd.Parameters.AddWithValue("@RequestID", userConnection.RequestID);  //Current Login User
                    cmd.Parameters.AddWithValue("@AcceptID", userConnection.AcceptID);
                    cmd.ExecuteNonQuery();
                }
                msg = "Data inserted successfully of the Request";
                res = true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                res = false;
            }
            return Ok(new { msg, res });
        }

        #endregion

        #region "Pending Notification Code"
        [HttpPost]
        public IActionResult PendingDBNotification([FromBody] UserPendingNotification notification)
        {
            bool res = false;
            string msg = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("", 1);
                    cmd.Parameters.AddWithValue("", 1);
                    cmd.Parameters.AddWithValue("", 1);
                    cmd.Parameters.AddWithValue("", 1);
                    cmd.Parameters.AddWithValue("", 1);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                res = false;
            }
            return Ok(new { res, msg });

        }


        #endregion
    }
}
