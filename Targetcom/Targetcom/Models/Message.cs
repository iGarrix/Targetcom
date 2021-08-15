using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public bool isurlmessage { get; set; }
        public DateTime TimeStamp { get; set; }

        public string ProfileId { get; set; }
        public Profile Profile { get; set; }

        public int MessageGroupId { get; set; }
        public MessageGroup MessageGroup { get; set; }
    }
}
