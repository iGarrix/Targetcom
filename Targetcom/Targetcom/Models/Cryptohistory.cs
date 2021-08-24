using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models
{
    public class Cryptohistory
    {
        [Key]
        public int Id { get; set; }
        public string Rate { get; set; }
        public int SingleCrypt { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
