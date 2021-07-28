using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models
{
    public class Game
    {
        public Game()
        {
            ProfileGames = new HashSet<ProfileGame>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string GameUrl { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int TargetPrice { get; set; } = 0;

        public ICollection<ProfileGame> ProfileGames { get; set; }
    }
}
