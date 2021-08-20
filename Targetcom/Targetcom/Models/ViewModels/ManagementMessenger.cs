using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models.ViewModels
{
    public class ManagementMessenger
    {
        public IEnumerable<Profile> Profiles { get; set; }
        public MessageGroup SelectRoom { get; set; }
        public string State { get; set; }
    }
}
