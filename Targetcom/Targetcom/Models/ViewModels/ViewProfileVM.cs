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
    }
}
