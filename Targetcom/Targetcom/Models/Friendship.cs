using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models
{
    public class Friendship
    {
        [Key]
        public int Id { get; set; }
        public Profile Profile { get; set; }
        public string ProfileId { get; set; }
        public Profile Friend { get; set; }
        public string FriendId { get; set; }
        public string FriendStatus { get; set; }
    }
}
