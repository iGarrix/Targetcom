using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace Targetcom.Models
{
    public class Profile : IdentityUser
    {
        public Profile()
        {
            ProfileGames = new HashSet<ProfileGame>();
            ProfilePostages = new HashSet<ProfilePostage>();
            WritterPostages = new HashSet<ProfilePostage>();
            LikedProfilePostages = new HashSet<LikedProfilePostage>();
            SharedProfilePostages = new HashSet<SharedProfilePostage>();
            ProfilePostageComments = new HashSet<ProfilePostageComment>();
            Friendships = new HashSet<Friendship>();
            BannedProfiles = new HashSet<BannedProfile>();
            Cases = new HashSet<Case>();
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
        [MaxLength(100, ErrorMessage = "Your status is overflowed limit")]
        public string StudyGeoplace { get; set; }
        [Required]
        public string Gender { get; set; }
        public string UrlAvatar { get; set; }
        [Required]
        public DateTime CreateStamp { get; set; } = DateTime.Now;
        [Required]
        public DateTime Age { get; set; }
        public bool IsVerify { get; set; } = false;
        public bool IsPremium { get; set; } = false;
        public int TargetCoins { get; set; } = 0;
        [NotMapped]
        public string MusicListening { get; set; }
        [NotMapped]
        public string AfkStatus { get; set; } = "offline";

        /*Privacy*/

        public string Privacy { get; set; } = Env.PublicProfile;
        public bool IsShortDate { get; set; } = false;
        public bool VisibilityQuote { get; set; } = true;
        public bool VisibilityPostage { get; set; } = true;
        public bool VisibilitySharePostage { get; set; } = true;
        public bool VisibilityFriends { get; set; } = true;
        public bool VisibilityImages { get; set; } = true;
        public bool VisibilitySubscribers { get; set; } = true;
        public bool VisibilityAboutMe { get; set; } = true;
        public bool VisibilityCommerceData { get; set; } = true;
        public bool VisibilityRole { get; set; } = false;
        public bool IsNessessaredLikedPost { get; set; } = false;
        public bool IsNessessaredSharedPost { get; set; } = false;
        public bool IsNessessaredPublishPost { get; set; } = false;
        public bool IsNessessaredCommentedPost { get; set; } = false;

        public bool ImageVidget { get; set; } = false;
        public string RainbowMode { get; set; } = Env.RainbowNone;
        public bool IsDark { get; set; } = false;
        public string AvatarGridURL { get; set; } = Env.GridNone;
        public int Fragment { get; set; } = 0;
        public string DroppedAvatar { get; set; } = string.Empty;

        public int BannedId { get; set; }
        public BannedProfile Banned { get; set; }
        public ICollection<BannedProfile> BannedProfiles { get; set; }


        public ICollection<ProfileGame> ProfileGames { get; set; }
        public ICollection<ProfilePostage> ProfilePostages { get; set; }
        public ICollection<ProfilePostage> WritterPostages { get; set; }
        public ICollection<LikedProfilePostage> LikedProfilePostages { get; set; }
        public ICollection<SharedProfilePostage> SharedProfilePostages { get; set; }
        public ICollection<ProfilePostageComment> ProfilePostageComments { get; set; }
        public ICollection<Friendship> Friendships { get; set; }
        public ICollection<Case> Cases { get; set; }
    }
}
