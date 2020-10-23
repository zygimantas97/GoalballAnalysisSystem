using System;
using System.Collections.Generic;
using System.Text;
using GoalballAnalysisSystem.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoalballAnalysisSystem.API.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Player>(entity =>
            {
                entity.HasOne(e => e.IdentityUser)
                    .WithMany()
                    .HasForeignKey(e => e.IdentityUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FKC_Player_IdentityUser");
            });

            builder.Entity<Team>(entity =>
            {
                entity.HasOne(e => e.IdentityUser)
                    .WithMany()
                    .HasForeignKey(e => e.IdentityUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FKC_Team_IdentityUser");
            });

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                },
                new IdentityRole
                {
                    Name = "RegularUser",
                    NormalizedName = "REGULARUSER"
                },
                new IdentityRole
                {
                    Name = "PremiumUser",
                    NormalizedName = "PREMIUMUSER"
                });

            base.OnModelCreating(builder);
        }
    }
}
