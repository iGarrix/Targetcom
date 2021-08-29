using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Targetcom.Models.ViewModels
{
    public class ViewProfileVM
    {
        public Profile IdentityProfile { get; set; }
        public Profile FindedProfile { get; set; }
        public string Role { get; set; }
        public string MyRole { get; set; }

        public string ListItem { get; set; }

        public int Current_AllPost_Page { get; set; }
        public int Current_AllPost_Lenght { get; set; }

        public int Current_SharedPost_Page { get; set; }
        public int Current_SharedPost_Lenght { get; set; }

        public bool IsAllable { get; set; }
    }
}
