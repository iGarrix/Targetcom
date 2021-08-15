using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Targetcom.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string username, string message, string urlavatar, string SenderEmail, string ReceiverEmail)
        {         
            await Clients.All.SendAsync("ReceiveMessage", username, message, urlavatar, SenderEmail, ReceiverEmail);
        }
    }
}
