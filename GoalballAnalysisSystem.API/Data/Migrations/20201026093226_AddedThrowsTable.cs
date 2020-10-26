using Microsoft.EntityFrameworkCore.Migrations;

namespace GoalballAnalysisSystem.API.Data.Migrations
{
    public partial class AddedThrowsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Throw",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X1 = table.Column<int>(nullable: false),
                    Y1 = table.Column<int>(nullable: false),
                    X2 = table.Column<int>(nullable: false),
                    Y2 = table.Column<int>(nullable: false),
                    Speed = table.Column<double>(nullable: false),
                    GameId = table.Column<long>(nullable: false),
                    GamePlayerId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Throw", x => x.Id);
                    table.ForeignKey(
                        name: "FKC_Throw_Game",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FKC_Throw_GamePlayer",
                        column: x => x.GamePlayerId,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "25f759c7-8807-446d-b3a3-08ad1805c1c6", "ae96fcd3-5ca6-4ecc-97af-b40867aad16a", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "068bd1be-b485-4c9e-8280-961d4d971874", "c6de5e23-7cab-4505-bebe-30033d4bd467", "RegularUser", "REGULARUSER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e66c8ed0-30bf-4ff3-ae6f-8e86ca91c190", "63bc8dfa-ffcc-4060-acd1-36f5b47bdb75", "PremiumUser", "PREMIUMUSER" });

            migrationBuilder.CreateIndex(
                name: "IX_Throw_GameId",
                table: "Throw",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Throw_GamePlayerId",
                table: "Throw",
                column: "GamePlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Throw");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "068bd1be-b485-4c9e-8280-961d4d971874");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "25f759c7-8807-446d-b3a3-08ad1805c1c6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e66c8ed0-30bf-4ff3-ae6f-8e86ca91c190");

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
        }
    }
}
