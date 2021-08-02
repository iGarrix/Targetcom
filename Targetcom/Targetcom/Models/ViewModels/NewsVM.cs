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
    }
}
