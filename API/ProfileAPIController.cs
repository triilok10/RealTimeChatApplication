using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatApplication.Models;
using System.Data.SqlClient;

namespace RealTimeChatApplication.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProfileAPIController : ControllerBase
    {
        private readonly string _connectionString;

        public ProfileAPIController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CustomConnection");
        }

        [HttpGet]
        public IActionResult UpdateProfile(string UpdateProfileID = "")
        {
            bool res = false;
            string msg = "";

            ChatUser obj = new ChatUser();

            try
            {

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_ChatUser", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 3);
                    cmd.Parameters.AddWithValue("@ChatUserUpdate", UpdateProfileID);
                    cmd.Parameters.AddWithValue("@ChatUser", DBNull.Value);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            obj.ChatUserID = Convert.ToInt32(rdr["ChatUserID"]);
                            obj.UserName = Convert.ToString(rdr["UserName"]);
                            obj.FullName = Convert.ToString(rdr["FullName"]);
                            obj.Email = Convert.ToString(rdr["Email"]);
                            obj.Gender = (ChatUser.GenderType?)Convert.ToInt16(rdr["Gender"]);
                        }
                        obj.Success = true;
                    }


                }
            }
            catch (Exception ex)
            {
                res = false;
                msg = ex.Message;
                obj.Success = false;
            }
            return Ok(obj);
        }
    }
}
