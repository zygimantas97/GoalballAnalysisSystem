using Microsoft.EntityFrameworkCore.Migrations;

namespace GoalballAnalysisSystem.API.Data.Migrations
{
    public partial class AddedTeamPlayersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1cb08204-2784-4fb7-a1c5-0fce0a7361f8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39da364a-2f56-47df-8a61-c9ba4b968b99");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b61850b8-b698-481e-af78-69cf16569b51");

            migrationBuilder.CreateTable(
                name: "PlayerRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamPlayers",
                columns: table => new
                {
                    TeamId = table.Column<long>(nullable: false),
                    PlayerId = table.Column<long>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    RoleId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamPlayer", x => new { x.TeamId, x.PlayerId });
                    table.ForeignKey(
                        name: "FKC_TeamPlayer_Player",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKC_TeamPlayer_PlayerRole",
                        column: x => x.RoleId,
                        principalTable: "PlayerRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FKC_TeamPlayer_Team",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cb8a2837-28bf-404b-8bdf-90c17a9d97e8", "d0f44e1a-591d-4dbe-8fe7-916ee4281df7", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "054cb964-3d7c-43e4-b4ad-4dd313030229", "07d0af0e-7978-4a2e-a1da-e74361263d75", "RegularUser", "REGULARUSER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2df59af7-685f-46d2-8959-76ca668a572a", "d35f08e0-9ae7-46a5-9953-16ac7b6f2085", "PremiumUser", "PREMIUMUSER" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamPlayers_PlayerId",
                table: "TeamPlayers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamPlayers_RoleId",
                table: "TeamPlayers",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamPlayers");

            migrationBuilder.DropTable(
                name: "PlayerRoles");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "054cb964-3d7c-43e4-b4ad-4dd313030229");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2df59af7-685f-46d2-8959-76ca668a572a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb8a2837-28bf-404b-8bdf-90c17a9d97e8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b61850b8-b698-481e-af78-69cf16569b51", "3582be47-391a-46cf-96e0-d4f9c04599cb", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "39da364a-2f56-47df-8a61-c9ba4b968b99", "9ba84dff-034b-4adb-a34f-838f31ad0199", "RegularUser", "REGULARUSER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1cb08204-2784-4fb7-a1c5-0fce0a7361f8", "6ccb6e34-92ff-4861-b04b-11e194b5af70", "PremiumUser", "PREMIUMUSER" });
        }
    }
}
