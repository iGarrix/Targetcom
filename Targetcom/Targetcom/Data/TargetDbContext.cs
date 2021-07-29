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
        }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<ProfileGame> ProfileGames { get; set; }
    }
}
