using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models
{
    public class SharedProfilePostage
    {
        public string ProfileId { get; set; }
        public int ProfilePostageId { get; set; }

        public Profile Profile { get; set; }
        public ProfilePostage Postage { get; set; }
    }
}
