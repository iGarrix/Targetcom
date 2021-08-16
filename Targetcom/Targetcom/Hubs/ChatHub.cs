using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Targetcom.Data;
using Targetcom.Models;
namespace Targetcom.Hubs
{
    public class ChatHub : Hub
    {
        

        private readonly TargetDbContext _db;
        public ChatHub(TargetDbContext db)
        {
            _db = db;
        }
        public async Task SendMessage(string username, string message, string urlavatar, string SenderEmail, string ReceiverEmail)
        {
            string SenderId = _db.Profiles.FirstOrDefault(f => f.Email == SenderEmail).Id;
            string ReceiverId = _db.Profiles.FirstOrDefault(f => f.Email == ReceiverEmail).Id;
            if (SenderId is not null && ReceiverId is not null)
            {
                var Room = _db.MessageGroups.FirstOrDefault(f => (f.AdminId == SenderId && f.FriendId == ReceiverId) || (f.AdminId == ReceiverId && f.FriendId == SenderId));
                if (Room is not null)
                {
                    if (_db.Messages.Where(w => w.MessageGroupId == Room.Id).Count() > Env.MAX_MESSAGES)
                    {
                        var messages = _db.Messages.Where(w => w.MessageGroupId == Room.Id).OrderBy(o => o.TimeStamp).FirstOrDefault();
                        if (message is not null)
                        {
                            _db.Messages.Remove(messages);
                        }
                    }
                    _db.Messages.Add(new Message()
                    {
                        TimeStamp = DateTime.Now,
                        Content = message,
                        isurlmessage = false,
                        Profile = _db.Profiles.Find(SenderId),
                        MessageGroup = Room,
                    });
                    await _db.SaveChangesAsync();
                }
                await Clients.Users(SenderId, ReceiverId).SendAsync("ReceiveMessage", username, message, urlavatar, SenderEmail, ReceiverEmail);
            }
        }
    }
}
