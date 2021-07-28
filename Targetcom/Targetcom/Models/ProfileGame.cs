using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models
{
    public class ProfileGame
    {
        public string ProfileId { get; set; }
        public int GameId { get; set; }

        public string Status { get; set; } = "None";

        [ForeignKey("ProfileId")]
        public Profile Profile { get; set; }
        [ForeignKey("GameId")]
        public Game Game { get; set; }
    }
}
