using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models
{
    public class Case
    {
        [Key]
        public int Id { get; set; }

        public string CaseType { get; set; }
        public int CaseCount { get; set; }

        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
