using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models.ViewModels
{
    public class ProfilePostComponentVM
    {
        public Profile IdentityProfile { get; set; }
        public string ViewProfileId { get; set; }
        public ProfilePostage Postage { get; set; }
        public Guid Guid { get; set; }
        public bool IsViewedProfile { get; set; }
    }
}
