using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models.ViewModels
{
    public class ProfileVM
    {
        public Profile IdentityProfile { get; set; }
        public IEnumerable<Profile> Friends { get; set; }
    }
}
