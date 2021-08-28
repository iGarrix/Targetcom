using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models.ViewModels
{
    public class NewsVM
    {
        public Profile IdentityProfile { get; set; }
        public IEnumerable<Profile> AllUsers { get; set; }
        public IEnumerable<ProfilePostage> ProfilePostages { get; set; }
        public string ListItem { get; set; }

        public int Current_AllPost_Page { get; set; }
        public int Current_AllPost_Lenght { get; set; }

        public int Current_TextPost_Page { get; set; }
        public int Current_TextPost_Lenght { get; set; }

        public int Current_Updates_Page { get; set; }
        public int Current_Updates_Lenght { get; set; }

        public int Current_Api_Page { get; set; }
        public int Current_Api_Lenght { get; set; }
    }
}
