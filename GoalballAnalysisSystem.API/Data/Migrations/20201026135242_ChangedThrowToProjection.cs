using Microsoft.EntityFrameworkCore.Migrations;

namespace GoalballAnalysisSystem.API.Data.Migrations
{
    public partial class ChangedThrowToProjection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Projection",
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
                    table.PrimaryKey("PK_Projection", x => x.Id);
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
                values: new object[] { "eac6f75e-9a2d-475b-a406-c50ee3679ecb", "b5b9a349-7306-4b5b-a66f-5406188a6301", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e8d3cb73-d6aa-45a3-be6d-78d0afc634e3", "56a8d8a2-efed-42f8-af55-39b6af549345", "RegularUser", "REGULARUSER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d719538c-a563-4cfb-abda-cabc8989ddd2", "e09e2d3f-c9e7-4a39-9e3d-692630e509d9", "PremiumUser", "PREMIUMUSER" });

            migrationBuilder.CreateIndex(
                name: "IX_Projection_GameId",
                table: "Projection",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Projection_GamePlayerId",
                table: "Projection",
                column: "GamePlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projection");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d719538c-a563-4cfb-abda-cabc8989ddd2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e8d3cb73-d6aa-45a3-be6d-78d0afc634e3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eac6f75e-9a2d-475b-a406-c50ee3679ecb");

            migrationBuilder.CreateTable(
                name: "Throw",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    GamePlayerId = table.Column<long>(type: "bigint", nullable: true),
                    Speed = table.Column<double>(type: "float", nullable: false),
                    X1 = table.Column<int>(type: "int", nullable: false),
                    X2 = table.Column<int>(type: "int", nullable: false),
                    Y1 = table.Column<int>(type: "int", nullable: false),
                    Y2 = table.Column<int>(type: "int", nullable: false)
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
    }
}
