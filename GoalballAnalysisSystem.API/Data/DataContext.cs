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

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
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
