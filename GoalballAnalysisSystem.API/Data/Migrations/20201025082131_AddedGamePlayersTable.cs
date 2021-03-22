using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoalballAnalysisSystem.API.Data.Migrations
{
    public partial class AddedGamePlayersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayers_TeamPlayers_TeamPlayerTeamId_TeamPlayerPlayerId",
                table: "GamePlayers");

            migrationBuilder.DropForeignKey(
                name: "FKC_TeamPlayer_PlayerRole",
                table: "TeamPlayers");

            migrationBuilder.DropForeignKey(
                name: "FKC_TeamPlayer_Team",
                table: "TeamPlayers");

            migrationBuilder.DropIndex(
                name: "IX_GamePlayers_TeamPlayerTeamId_TeamPlayerPlayerId",
                table: "GamePlayers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a5a80b8-6f60-439e-bcad-880f1d13cc27");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c7b28cf7-fe72-4a77-9b2e-d3522ee7f07d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e789c608-f6d2-48d4-bbec-cf4d4b099403");

            migrationBuilder.DropColumn(
                name: "TeamPlayerPlayerId",
                table: "GamePlayers");

            migrationBuilder.DropColumn(
                name: "TeamPlayerTeamId",
                table: "GamePlayers");

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    IdentityUserId = table.Column<string>(nullable: true),
                    HomeTeamId = table.Column<long>(nullable: true),
                    GuestTeamId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FKC_Game_GuestTeam",
                        column: x => x.GuestTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKC_Game_HomeTeam",
                        column: x => x.HomeTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKC_Game_IdentityUser",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "27e9d9c7-7ae0-4d05-b7b6-1f145a60a50f", "23086020-c303-42b0-b6b0-d017b0d8f656", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7624ec94-b21b-44d7-b91f-b36a740c10b6", "8328f510-2c5f-4b2f-a10f-d38a59fcd7c5", "RegularUser", "REGULARUSER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ba714f97-a33c-4867-84fa-f209d442722b", "38c9727e-4362-4222-baa7-810fcb520efb", "PremiumUser", "PREMIUMUSER" });

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_GameId",
                table: "GamePlayers",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_TeamId_PlayerId",
                table: "GamePlayers",
                columns: new[] { "TeamId", "PlayerId" });

            migrationBuilder.CreateIndex(
                name: "IX_Games_GuestTeamId",
                table: "Games",
                column: "GuestTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_HomeTeamId",
                table: "Games",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_IdentityUserId",
                table: "Games",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FKC_GamePlayer_Game",
                table: "GamePlayers",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FKC_GamePlayer_TeamPlayer",
                table: "GamePlayers",
                columns: new[] { "TeamId", "PlayerId" },
                principalTable: "TeamPlayers",
                principalColumns: new[] { "TeamId", "PlayerId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FKC_TeamPlayer_PlayerRole",
                table: "TeamPlayers",
                column: "RoleId",
                principalTable: "PlayerRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FKC_TeamPlayer_Team",
                table: "TeamPlayers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FKC_GamePlayer_Game",
                table: "GamePlayers");

            migrationBuilder.DropForeignKey(
                name: "FKC_GamePlayer_TeamPlayer",
                table: "GamePlayers");

            migrationBuilder.DropForeignKey(
                name: "FKC_TeamPlayer_PlayerRole",
                table: "TeamPlayers");

            migrationBuilder.DropForeignKey(
                name: "FKC_TeamPlayer_Team",
                table: "TeamPlayers");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropIndex(
                name: "IX_GamePlayers_GameId",
                table: "GamePlayers");

            migrationBuilder.DropIndex(
                name: "IX_GamePlayers_TeamId_PlayerId",
                table: "GamePlayers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27e9d9c7-7ae0-4d05-b7b6-1f145a60a50f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7624ec94-b21b-44d7-b91f-b36a740c10b6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ba714f97-a33c-4867-84fa-f209d442722b");

            migrationBuilder.AddColumn<long>(
                name: "TeamPlayerPlayerId",
                table: "GamePlayers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TeamPlayerTeamId",
                table: "GamePlayers",
                type: "bigint",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7a5a80b8-6f60-439e-bcad-880f1d13cc27", "9cbfe152-31fa-45f9-8847-a51ef087b2cb", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e789c608-f6d2-48d4-bbec-cf4d4b099403", "cdde5c83-a251-4e72-bd99-f357ebfdbeb6", "RegularUser", "REGULARUSER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c7b28cf7-fe72-4a77-9b2e-d3522ee7f07d", "1082a22f-d259-475b-a180-2de07511a1e4", "PremiumUser", "PREMIUMUSER" });

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_TeamPlayerTeamId_TeamPlayerPlayerId",
                table: "GamePlayers",
                columns: new[] { "TeamPlayerTeamId", "TeamPlayerPlayerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayers_TeamPlayers_TeamPlayerTeamId_TeamPlayerPlayerId",
                table: "GamePlayers",
                columns: new[] { "TeamPlayerTeamId", "TeamPlayerPlayerId" },
                principalTable: "TeamPlayers",
                principalColumns: new[] { "TeamId", "PlayerId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FKC_TeamPlayer_PlayerRole",
                table: "TeamPlayers",
                column: "RoleId",
                principalTable: "PlayerRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FKC_TeamPlayer_Team",
                table: "TeamPlayers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
