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
    }
}
