using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Targetcom.Models;

namespace Targetcom.Data
{
    public class TargetDbContext : IdentityDbContext
    {
        public TargetDbContext(DbContextOptions<TargetDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProfileGame>()
             .HasKey(e => new { e.ProfileId, e.GameId });

            builder.Entity<LikedProfilePostage>()
             .HasKey(e => new { e.ProfileId, e.ProfilePostageId });

            builder.Entity<SharedProfilePostage>()
             .HasKey(e => new { e.ProfileId, e.ProfilePostageId });

            /* ------- */

            builder.Entity<Profile>()
                .HasOne(e => e.Banned)
                .WithOne(w => w.Profile)
                .HasForeignKey<BannedProfile>(f => f.ProfileId);

            builder.Entity<Profile>()
                .HasMany(e => e.BannedProfiles)
                .WithOne(w => w.Admin)
                .HasForeignKey(f => f.AdminId);
                

            builder.Entity<Friendship>()
                .HasOne(fs => fs.Profile)
                .WithMany(u => u.Friendships)
                .HasForeignKey(fs => fs.ProfileId);
                

            builder.Entity<Profile>()
                .HasMany(e => e.ProfilePostages)
                .WithOne(w => w.Profile)
                .HasForeignKey(f => f.ProfileId);


            builder.Entity<Profile>()
                .HasMany(e => e.WritterPostages)
                .WithOne(w => w.Writter)
                .HasForeignKey(f => f.WritterId);


            builder.Entity<Profile>()
                .HasMany(m => m.LikedProfilePostages)
                .WithOne(w => w.Profile)
                .HasForeignKey(f => f.ProfileId);

            builder.Entity<Profile>()
                .HasMany(m => m.SharedProfilePostages)
                .WithOne(w => w.Profile)
                .HasForeignKey(f => f.ProfileId);

            builder.Entity<Profile>()
                .HasMany(m => m.ProfilePostageComments)
                .WithOne(w => w.ProfileCommentator)
                .HasForeignKey(f => f.ProfileCommentatorId);

            builder.Entity<ProfilePostage>()
                .HasMany(m => m.LikedProfiles)
                .WithOne(w => w.Postage)
                .HasForeignKey(f => f.ProfilePostageId);

            builder.Entity<ProfilePostage>()
               .HasMany(m => m.SharedProfiles)
               .WithOne(w => w.Postage)
               .HasForeignKey(f => f.ProfilePostageId);

            builder.Entity<ProfilePostage>()
              .HasMany(m => m.ProfilePostageComments)
              .WithOne(w => w.Postage)
              .HasForeignKey(f => f.PostageId);

        }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<ProfileGame> ProfileGames { get; set; }
        public DbSet<ProfilePostage> ProfilePostages { get; set; }
        public DbSet<LikedProfilePostage> LikedProfilePostages { get; set; }
        public DbSet<SharedProfilePostage> SharedProfilePostages { get; set; }
        public DbSet<ProfilePostageComment> ProfilePostageComments { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<BannedProfile> BannedProfiles { get; set; }
    }
}
