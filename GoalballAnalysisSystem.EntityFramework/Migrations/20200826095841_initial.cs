using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoalballAnalysisSystem.EntityFramework.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    SubscriptionExpire = table.Column<DateTime>(nullable: false),
                    Role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "fkc_user_role",
                        column: x => x.Role,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    User = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "fkc_player_user",
                        column: x => x.User,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    User = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "fkc_team_user",
                        column: x => x.User,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    User = table.Column<int>(nullable: false),
                    Team1 = table.Column<int>(nullable: true),
                    Team2 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "fkc_game_team1",
                        column: x => x.Team1,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fkc_game_team2",
                        column: x => x.Team2,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fkc_game_user",
                        column: x => x.User,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamPlayers",
                columns: table => new
                {
                    Team = table.Column<int>(nullable: false),
                    Player = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Role = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_team_player", x => new { x.Team, x.Player });
                    table.ForeignKey(
                        name: "fkc_teamplayer_player",
                        column: x => x.Player,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fkc_player_role",
                        column: x => x.Role,
                        principalTable: "PlayerRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fkc_teamplayer_team",
                        column: x => x.Team,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GamePlayers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Team = table.Column<int>(nullable: false),
                    Player = table.Column<int>(nullable: false),
                    Game = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayers", x => x.Id);
                    table.ForeignKey(
                        name: "fkc_gameplayer_game",
                        column: x => x.Game,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fkc_gameplayer_teamplayer",
                        columns: x => new { x.Team, x.Player },
                        principalTable: "TeamPlayers",
                        principalColumns: new[] { "Team", "Player" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Throws",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X1 = table.Column<int>(nullable: false),
                    Y1 = table.Column<int>(nullable: false),
                    X2 = table.Column<int>(nullable: false),
                    Y2 = table.Column<int>(nullable: false),
                    Speed = table.Column<double>(nullable: false),
                    Game = table.Column<int>(nullable: false),
                    GamePlayer = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Throws", x => x.Id);
                    table.ForeignKey(
                        name: "fkc_throw_game",
                        column: x => x.Game,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fkc_throw_player",
                        column: x => x.GamePlayer,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_Game",
                table: "GamePlayers",
                column: "Game");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_Team_Player",
                table: "GamePlayers",
                columns: new[] { "Team", "Player" });

            migrationBuilder.CreateIndex(
                name: "IX_Games_Team1",
                table: "Games",
                column: "Team1");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Team2",
                table: "Games",
                column: "Team2");

            migrationBuilder.CreateIndex(
                name: "IX_Games_User",
                table: "Games",
                column: "User");

            migrationBuilder.CreateIndex(
                name: "IX_Players_User",
                table: "Players",
                column: "User");

            migrationBuilder.CreateIndex(
                name: "IX_TeamPlayers_Player",
                table: "TeamPlayers",
                column: "Player");

            migrationBuilder.CreateIndex(
                name: "IX_TeamPlayers_Role",
                table: "TeamPlayers",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_User",
                table: "Teams",
                column: "User");

            migrationBuilder.CreateIndex(
                name: "IX_Throws_Game",
                table: "Throws",
                column: "Game");

            migrationBuilder.CreateIndex(
                name: "IX_Throws_GamePlayer",
                table: "Throws",
                column: "GamePlayer");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role",
                table: "Users",
                column: "Role");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Throws");

            migrationBuilder.DropTable(
                name: "GamePlayers");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "TeamPlayers");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "PlayerRoles");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserRoles");
        }
    }
}
