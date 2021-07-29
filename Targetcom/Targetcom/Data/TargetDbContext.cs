using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

            builder.Entity<Profile>()
                .HasMany(e => e.ProfilePostages)
                .WithOne(w => w.Profile);
        }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<ProfileGame> ProfileGames { get; set; }
        public DbSet<ProfilePostage> ProfilePostages { get; set; }
        public DbSet<LikedProfilePostage> LikedProfilePostages { get; set; }
        public DbSet<ProfilePostageComment> ProfilePostageComments { get; set; }
    }
}
