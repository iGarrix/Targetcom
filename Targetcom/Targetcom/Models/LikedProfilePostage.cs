using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models
{
    public class LikedProfilePostage
    {
        public string ProfileId { get; set; }
        public int ProfilePostageId { get; set; }

        [ForeignKey("ProfileId")]
        public Profile Profile { get; set; }
        [ForeignKey("ProfilePostageId")]
        public ProfilePostage Postage { get; set; }
    }
}
