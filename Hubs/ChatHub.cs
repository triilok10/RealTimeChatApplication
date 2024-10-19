using Microsoft.AspNetCore.SignalR;
using System.Data.SqlClient;
using System.Threading.Tasks;
using RealTimeChatApplication.AppCode;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RealTimeChatApplication.Hubs
{
    public class ChatHub : Hub
    {
        private readonly string _connectionString;
        private readonly ISessionService _sessionService;

        private static readonly ConcurrentDictionary<string, List<string>> _userConnections = new ConcurrentDictionary<string, List<string>>();

        public ChatHub(IConfiguration configuration, ISessionService sessionService)
        {
            _connectionString = configuration.GetConnectionString("CustomConnection");
            _sessionService = sessionService;
        }

        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            _sessionService.SetString("connectionId", connectionId);

            var userId = _sessionService.GetInt32("UserID");
            string userID = Convert.ToString(userId);

            _userConnections.AddOrUpdate(userID, new List<string> { connectionId }, (key, existingList) =>
            {
                existingList.Add(connectionId);
                return existingList;
            });

            Console.WriteLine($"User {userId} connected with Connection ID: {connectionId}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;
            var userId = _sessionService.GetInt32("UserID");
            string userID = Convert.ToString(userId);

            if (_userConnections.TryGetValue(userID, out var connections))
            {
                connections.Remove(connectionId);

                if (connections.Count == 0)
                {
                    _userConnections.TryRemove(userID, out _);
                }
            }

            Console.WriteLine($"User {userId} disconnected with Connection ID: {connectionId}");

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string recipientUserId, string message)
        {
            await SaveMessageToDatabase(recipientUserId, message);

            var senderUserName = _sessionService.GetString("UserName");

            if (_userConnections.TryGetValue(recipientUserId, out var connectionIds))
            {
                foreach (var connectionId in connectionIds)
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderUserName, message);
                }
            }
        }

        private async Task SaveMessageToDatabase(string recipientId, string message)
        {
            try
            {
                var senderUserId = _sessionService.GetInt32("UserID");

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("usp_MessageRecord", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", 1);
                        cmd.Parameters.AddWithValue("@SenderID", senderUserId);
                        cmd.Parameters.AddWithValue("@ReceiverID", recipientId);
                        cmd.Parameters.AddWithValue("@Message", message);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving message: {ex.Message}");
            }
        }
    }
}
