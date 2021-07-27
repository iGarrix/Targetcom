using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string GameUrl { get; set; }
    }
}
