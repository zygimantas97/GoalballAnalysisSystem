using Microsoft.EntityFrameworkCore.Migrations;

namespace GoalballAnalysisSystem.API.Data.Migrations
{
    public partial class AddedOffenseAndDefenseProjections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FKC_Throw_Game",
                table: "Projection");

            migrationBuilder.DropForeignKey(
                name: "FKC_Throw_GamePlayer",
                table: "Projection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projection",
                table: "Projection");

            migrationBuilder.DropIndex(
                name: "IX_Projection_GamePlayerId",
                table: "Projection");

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

            migrationBuilder.DropColumn(
                name: "GamePlayerId",
                table: "Projection");

            migrationBuilder.RenameTable(
                name: "Projection",
                newName: "Projections");

            migrationBuilder.RenameIndex(
                name: "IX_Projection_GameId",
                table: "Projections",
                newName: "IX_Projections_GameId");

            migrationBuilder.AddColumn<long>(
                name: "DefenseGamePlayerId",
                table: "Projections",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OffenseGamePlayerId",
                table: "Projections",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projections",
                table: "Projections",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8d2d1e27-3fd7-443b-b7b8-89312e001192", "b7cdbee1-864c-46e4-928b-e85533221219", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f33abb43-17fd-4d76-9927-e8eaac45baa3", "847ecf9b-f371-4f03-ba8c-90182e7997dd", "RegularUser", "REGULARUSER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "dc20816d-d380-452e-84e9-218f97ba1347", "5885ae58-151d-4400-a60d-c9e98304df1c", "PremiumUser", "PREMIUMUSER" });

            migrationBuilder.CreateIndex(
                name: "IX_Projections_DefenseGamePlayerId",
                table: "Projections",
                column: "DefenseGamePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Projections_OffenseGamePlayerId",
                table: "Projections",
                column: "OffenseGamePlayerId");

            migrationBuilder.AddForeignKey(
                name: "FKC_Projection_DefenseGamePlayer",
                table: "Projections",
                column: "DefenseGamePlayerId",
                principalTable: "GamePlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FKC_Projection_Game",
                table: "Projections",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FKC_Projection_OffenseGamePlayer",
                table: "Projections",
                column: "OffenseGamePlayerId",
                principalTable: "GamePlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FKC_Projection_DefenseGamePlayer",
                table: "Projections");

            migrationBuilder.DropForeignKey(
                name: "FKC_Projection_Game",
                table: "Projections");

            migrationBuilder.DropForeignKey(
                name: "FKC_Projection_OffenseGamePlayer",
                table: "Projections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projections",
                table: "Projections");

            migrationBuilder.DropIndex(
                name: "IX_Projections_DefenseGamePlayerId",
                table: "Projections");

            migrationBuilder.DropIndex(
                name: "IX_Projections_OffenseGamePlayerId",
                table: "Projections");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d2d1e27-3fd7-443b-b7b8-89312e001192");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dc20816d-d380-452e-84e9-218f97ba1347");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f33abb43-17fd-4d76-9927-e8eaac45baa3");

            migrationBuilder.DropColumn(
                name: "DefenseGamePlayerId",
                table: "Projections");

            migrationBuilder.DropColumn(
                name: "OffenseGamePlayerId",
                table: "Projections");

            migrationBuilder.RenameTable(
                name: "Projections",
                newName: "Projection");

            migrationBuilder.RenameIndex(
                name: "IX_Projections_GameId",
                table: "Projection",
                newName: "IX_Projection_GameId");

            migrationBuilder.AddColumn<long>(
                name: "GamePlayerId",
                table: "Projection",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projection",
                table: "Projection",
                column: "Id");

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
                name: "IX_Projection_GamePlayerId",
                table: "Projection",
                column: "GamePlayerId");

            migrationBuilder.AddForeignKey(
                name: "FKC_Throw_Game",
                table: "Projection",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FKC_Throw_GamePlayer",
                table: "Projection",
                column: "GamePlayerId",
                principalTable: "GamePlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
