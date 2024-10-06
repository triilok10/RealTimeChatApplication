using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatApplication.Models;
using System.Data.SqlClient;
using static RealTimeChatApplication.Models.ChatUser;

namespace RealTimeChatApplication.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginAPIController : ControllerBase
    {
        private readonly string _connectionString;

        public LoginAPIController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CustomConnection");
        }



        [HttpPost]
        public IActionResult Register([FromBody] ChatUser pChatUser)
        {
            bool res = false;
            string msg = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_ChatUser", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", 1);
                        cmd.Parameters.AddWithValue("@UserName", pChatUser.UserName);
                        cmd.Parameters.AddWithValue("@Password", pChatUser.Password);
                        cmd.Parameters.AddWithValue("@Email", pChatUser.Email);
                        cmd.Parameters.AddWithValue("@Gender", pChatUser.Gender);
                        cmd.Parameters.AddWithValue("@ProfilePictureURL", pChatUser.HdnProfilePicture);
                        cmd.ExecuteNonQuery();


                        //using (SqlDataReader rdr = cmd.ExecuteReader())
                        //{
                        //    while (rdr.Read())
                        //    {
                        //        pChatUser.ChatUserID = Convert.ToInt32("");
                        //    }
                        //}
                    }
                }
                res = true;
                msg = "User Created Successfully";
            }
            catch (Exception ex)
            {
                res = false;
                msg = ex.Message;
            }
            return Ok(new { response = res, message = msg });
        }
    }
}
