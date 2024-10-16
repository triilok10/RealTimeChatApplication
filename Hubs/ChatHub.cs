﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace RealTimeChatApplication.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string recipientUserId, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", recipientUserId, message);
        }

        //public async Task SendMessage( string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", message);
        //}
    }
}
