using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using RealTimeChatApplication.AppCode;
using RealTimeChatApplication.Models;
using System.Data.SqlClient;

namespace RealTimeChatApplication.Hubs
{
    public class ChatHub : Hub
    {

        private readonly string _connectionString;
        private readonly ISessionService _sessionService;
        public ChatHub(IConfiguration configuration, ISessionService sessionService)
        {
            _connectionString = configuration.GetConnectionString("CustomConnection");
            _sessionService = sessionService;
        }


        public async Task SendMessage(string recipientUserId, string message)
        {

            await SaveMessageToDatabase(recipientUserId, message);


            await Clients.User(recipientUserId).SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        }

        private async Task SaveMessageToDatabase(string recipientId, string message)
        {
            string msg = "";
            try
            {

                var SenderUserId = _sessionService.GetInt32("UserID");
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("usp_MessageRecord", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", 1);
                    cmd.Parameters.AddWithValue("@SenderID", SenderUserId);
                    cmd.Parameters.AddWithValue("@ReceiverID", recipientId);
                    cmd.Parameters.AddWithValue("@Message", message);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }
    }
}
