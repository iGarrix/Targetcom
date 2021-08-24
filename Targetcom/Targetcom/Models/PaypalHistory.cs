using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models
{
    public class PaypalHistory
    {
        [Key]
        public int Id { get; set; }
        public string PaySummary { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime TransactionTime { get; set; } = DateTime.Now;
        public string ValueName { get; set; }
        public int ValueCount { get; set; }

        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
