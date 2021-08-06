using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models
{
    public class BannedProfile
    {
        [Key]
        public int Id { get; set; }

        public string ProfileId { get; set; }
        public Profile Profile { get; set; }

        public string AdminId { get; set; }
        public Profile Admin { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public string Reason { get; set; } = "Violation of site rights";

        public bool IsPermanent { get; set; } = false;
        public DateTime ReasonDate { get; set; } = DateTime.Now.AddHours(3);
    }
}
