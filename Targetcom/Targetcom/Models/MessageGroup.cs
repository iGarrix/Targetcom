using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models
{
    public class MessageGroup
    {
        public MessageGroup()
        {
            Messages = new HashSet<Message>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsInvite { get; set; }

        public string AdminId { get; set; }
        public Profile Admin { get; set; }

        public string FriendId { get; set; }
        public Profile Friend { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
