using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
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
                    cmd.Parameters.AddWithValue("@SearchUserName", pChatMessage.SearchConnection);
                    cmd.Parameters.AddWithValue("@SearchUserID", pChatMessage.ChatMessageID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ChatMessage obj = new ChatMessage
                            {
                                UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : string.Empty,
                                FullName = reader["FullName"] != DBNull.Value ? Convert.ToString(reader["FullName"]) : string.Empty,
                                ProfilePictureURL = reader["ProfilePictureURL"] != DBNull.Value ? Convert.ToString(reader["ProfilePictureURL"]) : string.Empty,
                                IsRequestAccepted = reader["IsRequestAccepted"] != DBNull.Value ? Convert.ToBoolean(reader["IsRequestAccepted"]) : false,
                                RequestID = reader["RequesterID"] != DBNull.Value ? Convert.ToInt32(reader["RequesterID"]) : 0,
                                AcceptID = reader["AccepterID"] != DBNull.Value ? Convert.ToInt32(reader["AccepterID"]) : 0,
                                Gender = reader["Gender"] != DBNull.Value ? (ChatMessage.GenderType?)Enum.ToObject(typeof(ChatMessage.GenderType), Convert.ToInt16(reader["Gender"])) : null,
                                Status = reader["RequestStatus"] != DBNull.Value ? Convert.ToString(reader["RequestStatus"]) : "",
                                ChatUserID = reader["ChatUserID"] != DBNull.Value ? Convert.ToInt32(reader["ChatUserID"]) : 0
                            };

                            lstMessage.Add(obj);
                        }
                    }
                    return Ok(lstMessage);
                }
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
                                ProfilePictureURL = Convert.ToString(rdr["ProfilePictureURL"]),
                                Gender = rdr["Gender"] != DBNull.Value ? (ChatMessage.GenderType?)Enum.ToObject(typeof(ChatMessage.GenderType), Convert.ToInt16(rdr["Gender"])) : null
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

        [HttpGet]
        public IActionResult SeeProfileData(int SeeProfileID = 0, int loginID = 0)
        {

            UserConnection user = new UserConnection();
            try
            {
                if (SeeProfileID > 0 && loginID > 0)
                {
                    using (SqlConnection con = new SqlConnection(_connectionString))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("usp_ChatMessage", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Mode", 5);
                        cmd.Parameters.AddWithValue("@ChatUserID", loginID);
                        cmd.Parameters.AddWithValue("@AcceptID", SeeProfileID);

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                user.RequestID = Convert.ToInt32(rdr["RequestID"]);
                                user.AcceptID = Convert.ToInt32(rdr["AcceptID"]);
                                user.IsRequestAccepted = Convert.ToBoolean(rdr["IsRequestAccepted"]);
                            }
                        }

                    }
                }
            }
            catch (Exception ex) { }
            return Ok(user);
        }


        [HttpGet]
        public IActionResult UserInfo(int SeeProfileID = 0)
        {

            ChatUser user = new ChatUser();
            try
            {
                if (SeeProfileID > 0)
                {
                    using (SqlConnection con = new SqlConnection(_connectionString))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("usp_ChatMessage", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Mode", 6);
                        cmd.Parameters.AddWithValue("@ChatUserID", SeeProfileID);

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                user.ChatUserID = Convert.ToInt32(rdr["ChatUserID"]);
                                user.FullName = Convert.ToString(rdr["FullName"]);
                                user.UserName = Convert.ToString(rdr["UserName"]);
                                user.Email = Convert.ToString(rdr["Email"]);
                                user.Gender = (ChatUser.GenderType?)Convert.ToInt16(rdr["Gender"]);
                            }
                        }

                    }
                }
            }
            catch (Exception ex) { }
            return Ok(user);
        }

        [HttpGet]
        public IActionResult LastLoginTime(string LastLoginTimeID = "")
        {
            bool res = false;
            string msg = "";

            try
            {
                if (string.IsNullOrWhiteSpace(LastLoginTimeID))
                {
                    res = false;
                    msg = "Please select the User";
                    return Ok(new { res, msg });
                }

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_ChatMessage", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 4);
                    cmd.Parameters.AddWithValue("@ChatUserID", LastLoginTimeID);

                    ChatMessage Obj = new ChatMessage();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Obj.TimeStamp = Convert.ToDateTime(rdr["LastLoginTime"]);
                        }
                        res = true;
                        msg = "Data received successfully";
                    }
                    return Ok(Obj);
                }
            }
            catch (Exception ex)
            {
                res = false;
                msg = ex.Message;
            }
            return Ok(new { res, msg });
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
                                ProfilePictureURL = Convert.ToString(rdr["ProfilePictureURL"]),
                                Gender = rdr["Gender"] != DBNull.Value ? (ChatMessage.GenderType?)Enum.ToObject(typeof(ChatMessage.GenderType), Convert.ToInt16(rdr["Gender"])) : null
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
                ChatUser obj = new ChatUser();
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_MessageRecord", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 4);
                    cmd.Parameters.AddWithValue("@SenderID", userConnection.RequestID);  //Current Login User
                    cmd.Parameters.AddWithValue("@ReceiverID", userConnection.AcceptID);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            obj.FCMToken = Convert.ToString(rdr["FCMToken"]);
                        }
                    }
                }
                msg = "Data inserted successfully of the Request";
                res = true;
                return Ok(new { msg, res, FCMToken = obj.FCMToken });
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                res = false;
                return Ok(new { msg, res });
            }

        }

        #endregion

        #region "Pending Notification Code"
        [HttpPost]
        public IActionResult NotificationMessage([FromBody] SendNotificationMessage notification)
        {
            bool res = false;
            string msg = "";

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_NotificationRecord", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 1);
                    cmd.Parameters.AddWithValue("@SenderID", notification.SenderID);
                    cmd.Parameters.AddWithValue("@ReceiverID", notification.ReceiverID);
                    cmd.Parameters.AddWithValue("@MessageData", notification.MessageData);
                    cmd.Parameters.AddWithValue("@IsNotificationSend", notification.IsNotificationSend);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                res = false;
            }
            return Ok(new { res = res, msg = msg });

        }

        [HttpPost]
        public IActionResult CancelRequest(UserConnection userConnection)
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
                    cmd.Parameters.AddWithValue("@Mode", 5);
                    cmd.Parameters.AddWithValue("@SenderID", userConnection.RequestID);
                    cmd.Parameters.AddWithValue("@ReceiverID", userConnection.AcceptID);
                    cmd.ExecuteNonQuery();
                    msg = "Connection request cancalled successfully";
                    res = true;
                    return Ok(new { msg, res });
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                res = false;
                return Ok(new { msg, res });
            }

        }
        #endregion



        [HttpPost]
        public IActionResult ChatVerifyUser([FromBody] ChatMessage pChatMessage)
        {
            bool res = false;
            string msg = "";
            ChatMessage obj = new ChatMessage();
            try
            {

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_ChatMessage", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", 3);
                    cmd.Parameters.AddWithValue("@AcceptID", pChatMessage.ChatMessageID);
                    cmd.Parameters.AddWithValue("@RequestID", pChatMessage.ChatReceiverID);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            obj.IsRequestAccepted = Convert.ToBoolean(rdr["IsRequestAccepted"]);
                        }
                    }
                    res = true;
                    msg = "User Connected";
                    return Ok(obj);
                }
            }
            catch (Exception ex)
            {
                res = false;
                msg = ex.Message;
                return Ok(new { data = obj, res = res, msg = msg });
            }


        }
    }
}
