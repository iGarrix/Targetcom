using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models
{
    public class ProfilePostageComment
    {
        [Key]
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public string ProfileCommentatorId { get; set; }
        public Profile ProfileCommentator { get; set; }

        public int PostageId { get; set; }
        public ProfilePostage Postage { get; set; }
    }
}
