using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models.ViewModels
{
    public class MessageVM
    {
        public Profile IdentityProfile { get; set; }
        public IEnumerable<Profile> Profiles { get; set; }
        public Profile SelectedProfile { get; set; } = null;
    }
}
