using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models.ViewModels
{
    public class ProfileVM
    {
        public Profile IdentityProfile { get; set; }
        public string ListItem { get; set; }

        public int Current_AllPost_Page { get; set; }
        public int Current_AllPost_Lenght { get; set; }

        public int Current_SharedPost_Page { get; set; }
        public int Current_SharedPost_Lenght { get; set; }

        public bool IsAllable { get; set; }
    }
}
