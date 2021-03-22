using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoalballAnalysisSystem.API.Data.Migrations
{
    public partial class RemoveGamesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "075dcd53-c9dc-41a0-bd27-580a9127c1be");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1981e719-6f94-4268-9a64-d9776a69cd3e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "da746397-9004-452d-85d3-41106635067b");

            migrationBuilder.CreateTable(
                name: "GamePlayers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    TeamId = table.Column<long>(nullable: false),
                    PlayerId = table.Column<long>(nullable: false),
                    GameId = table.Column<long>(nullable: false),
                    TeamPlayerTeamId = table.Column<long>(nullable: true),
                    TeamPlayerPlayerId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamePlayers_TeamPlayers_TeamPlayerTeamId_TeamPlayerPlayerId",
                        columns: x => new { x.TeamPlayerTeamId, x.TeamPlayerPlayerId },
                        principalTable: "TeamPlayers",
                        principalColumns: new[] { "TeamId", "PlayerId" },
                        onDelete: ReferentialAction.Restrict);
                });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamePlayers");

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

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuestTeamId = table.Column<long>(type: "bigint", nullable: true),
                    HomeTeamId = table.Column<long>(type: "bigint", nullable: true),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                values: new object[] { "1981e719-6f94-4268-9a64-d9776a69cd3e", "3dd6a755-376b-46f9-9dc7-247a6d85bf19", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "075dcd53-c9dc-41a0-bd27-580a9127c1be", "2bb12ab5-ba2a-48a9-9699-81882bd7cd64", "RegularUser", "REGULARUSER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "da746397-9004-452d-85d3-41106635067b", "a79d8cda-76f5-4698-934d-41bcc0db0b18", "PremiumUser", "PREMIUMUSER" });

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
        }
    }
}
