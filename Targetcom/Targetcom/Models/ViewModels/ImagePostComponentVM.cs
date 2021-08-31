using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models.ViewModels
{
    public class ImagePostComponentVM
    {
        public Profile IdentityProfile { get; set; }
        public ProfilePostage ImageModel { get; set; }
        public Guid Guid { get; set; }
    }
}
