using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models.ViewModels
{
    public class FriendVM
    {
        public Profile IdentityUser { get; set; }
        public IEnumerable<Profile> AllUsers { get; set; }
        public string EventSetter { get; set; }

        public int Current_Friend_Page { get; set; }
        public int Current_Friend_Lenght { get; set; }

        public int Current_Invite_Page { get; set; }
        public int Current_Invite_Lenght { get; set; }

        public int Current_Subscribe_Page { get; set; }
        public int Current_Subscribe_Lenght { get; set; }

        public int Current_Blacklist_Page { get; set; }
        public int Current_Blacklist_Lenght { get; set; }

        public int Current_All_Page { get; set; }
        public int Current_All_Lenght { get; set; }
    }
}
