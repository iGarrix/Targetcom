﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Models
{
    public class ProfilePostage
    {
        public ProfilePostage()
        {
            LikedProfiles = new HashSet<LikedProfilePostage>();
            SharedProfiles = new HashSet<SharedProfilePostage>();
            ProfilePostageComments = new HashSet<ProfilePostageComment>();
        }
        [Key]
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public bool IsObject { get; set; } = false;
        public string Content { get; set; }
        public string Alert { get; set; }
        public string UploadingUrlFiles { get; set; }
        public bool IsPinned { get; set; } = false;

        public bool IsNessessaredLikedPost { get; set; } = false;
        public bool IsNessessaredSharedPost { get; set; } = false;
        public bool IsNessessaredCommentedPost { get; set; } = false;

        public string WritterId { get; set; }
        public Profile Writter { get; set; }

        public string ProfileId { get; set; }
        public Profile Profile { get; set; }

        public ICollection<LikedProfilePostage> LikedProfiles { get; set; }
        public ICollection<SharedProfilePostage> SharedProfiles { get; set; }
        public ICollection<ProfilePostageComment> ProfilePostageComments { get; set; }      
    }
}
