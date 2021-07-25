using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace Targetcom.Models
{
    public class Profile : IdentityUser
    {
        public Profile()
        {

        }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [MaxLength(50, ErrorMessage = "Geolocation more then symbol limit")]
        public string JobGeoplace { get; set; }
        [MaxLength(50, ErrorMessage = "Your status is overflowed limit")]
        public string Status { get; set; }
        [MaxLength(100, ErrorMessage = "Your hobbies is overflowed limit")]
        public string Hobbies { get; set; }
        [MaxLength(100, ErrorMessage = "Your status is overflowed limit")]
        public string Quote { get; set; }
        [Required]
        public string Gender { get; set; }
        public string UrlImage { get; set; }
        [Required]
        public DateTime CreateStamp { get; set; }
        [Required]
        public DateTime Age { get; set; }
        public bool IsVerify { get; set; }
    }
}
