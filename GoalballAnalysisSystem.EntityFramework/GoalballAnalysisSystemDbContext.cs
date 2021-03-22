using GoalballAnalysisSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace GoalballAnalysisSystem.EntityFramework
{
    public class GoalballAnalysisSystemDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Throw> Throws { get; set; }
        public DbSet<TeamPlayer> TeamPlayers { get; set; }
        public DbSet<GamePlayer> GamePlayers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<PlayerRole> PlayerRoles { get; set; }

        public GoalballAnalysisSystemDbContext(DbContextOptions options) : base(options)
        {

        }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GamePlayer>(entity =>
            {
                entity.HasOne(d => d.GameNavigation)
                    .WithMany(p => p.GamePlayers)
                    .HasForeignKey(d => d.Game)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("fkc_gameplayer_game");

                entity.HasOne(d => d.TeamPlayerNavigation)
                    .WithMany(p => p.GamePlayers)
                    .HasForeignKey(d => new { d.Team, d.Player })
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("fkc_gameplayer_teamplayer");
            });

            modelBuilder.Entity<TeamPlayer>(entity =>
            {
                entity.HasKey(e => new { e.Team, e.Player })
                    .HasName("pk_team_player");

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.TeamPlayers)
                    .HasForeignKey(d => d.Role)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkc_player_role");

                entity.HasOne(d => d.TeamNavigation)
                    .WithMany(p => p.TeamPlayers)
                    .HasForeignKey(d => d.Team)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("fkc_teamplayer_team");

                entity.HasOne(d => d.PlayerNavigation)
                    .WithMany(p => p.TeamPlayers)
                    .HasForeignKey(d => d.Player)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("fkc_teamplayer_player");
            });

            modelBuilder.Entity<Throw>(entity =>
            {
                entity.HasOne(d => d.GameNavigation)
                    .WithMany(p => p.Throws)
                    .HasForeignKey(d => d.Game)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("fkc_throw_game");

                entity.HasOne(d => d.GamePlayerNavigation)
                    .WithMany(p => p.Throws)
                    .HasForeignKey(d => d.GamePlayer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkc_throw_player");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("fkc_game_user");

                entity.HasOne(d => d.Team1Navigation)
                    .WithMany(p => p.Games1)
                    .HasForeignKey(d => d.Team1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkc_game_team1");


                entity.HasOne(d => d.Team2Navigation)
                    .WithMany(p => p.Games2)
                    .HasForeignKey(d => d.Team2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkc_game_team2");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("fkc_team_user");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Players)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("fkc_player_user");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Role)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("fkc_user_role");
            });

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    Id = 1,
                    Name = "Standard user"
                },
                new UserRole
                {
                    Id = 2,
                    Name = "Premium user"
                }) ;

            base.OnModelCreating(modelBuilder);
        }

    }
}
