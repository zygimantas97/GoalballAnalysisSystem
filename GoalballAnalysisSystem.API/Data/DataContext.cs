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
        public DbSet<PlayerRole> PlayerRoles { get; set; }
        public DbSet<TeamPlayer> TeamPlayers { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GamePlayer> GamePlayers { get; set; }
        public DbSet<Projection> Projections { get; set; }

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

            builder.Entity<TeamPlayer>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.PlayerId })
                    .HasName("PK_TeamPlayer");

                entity.HasOne(e => e.Team)
                    .WithMany(f => f.TeamPlayers)
                    .HasForeignKey(e => e.TeamId)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FKC_TeamPlayer_Team");

                entity.HasOne(e => e.Player)
                    .WithMany(f => f.PlayerTeams)
                    .HasForeignKey(e => e.PlayerId)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FKC_TeamPlayer_Player");

                entity.HasOne(e => e.Role)
                    .WithMany()
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FKC_TeamPlayer_PlayerRole");
            });
            
            builder.Entity<Game>(entity =>
            {
                entity.HasOne(e => e.IdentityUser)
                    .WithMany()
                    .HasForeignKey(e => e.IdentityUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FKC_Game_IdentityUser");

                entity.HasOne(e => e.HomeTeam)
                    .WithMany(f => f.HomeGames)
                    .HasForeignKey(e => e.HomeTeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKC_Game_HomeTeam");

                entity.HasOne(e => e.GuestTeam)
                    .WithMany(f => f.GuestGames)
                    .HasForeignKey(e => e.GuestTeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKC_Game_GuestTeam");
            });
            
            builder.Entity<GamePlayer>(entity =>
            {
                entity.HasOne(e => e.Game)
                    .WithMany(f => f.GamePlayers)
                    .HasForeignKey(e => e.GameId)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FKC_GamePlayer_Game");
                
                entity.HasOne(e => e.TeamPlayer)
                    .WithMany(f => f.GamePlayers)
                    .HasForeignKey(e => new { e.TeamId, e.PlayerId })
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FKC_GamePlayer_TeamPlayer");
            });

            builder.Entity<Projection>(entity =>
            {
                entity.HasOne(e => e.Game)
                    .WithMany(f => f.Throws)
                    .HasForeignKey(e => e.GameId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FKC_Throw_Game");

                entity.HasOne(e => e.GamePlayer)
                    .WithMany(f => f.Throws)
                    .HasForeignKey(e => e.GamePlayerId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FKC_Throw_GamePlayer");
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

            builder.Entity<PlayerRole>().HasData(
                new PlayerRole
                {
                    Id = 1,
                    Name = "LeftStriker"
                },
                new PlayerRole
                {
                    Id = 2,
                    Name = "RightStriker"
                },
                new PlayerRole
                {
                    Id = 3,
                    Name = "Center"
                });

            base.OnModelCreating(builder);
        }

        public DbSet<GoalballAnalysisSystem.API.Models.Projection> Throw { get; set; }
    }
}
