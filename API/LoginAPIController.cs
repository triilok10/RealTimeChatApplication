using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatApplication.Models;
using System.Data.SqlClient;

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
                    cmd.ExecuteNonQuery();
                }
                return Ok();
            }
        }
    }
}
