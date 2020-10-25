using Microsoft.EntityFrameworkCore.Migrations;

namespace GoalballAnalysisSystem.API.Data.Migrations
{
    public partial class ChangedPlayerOnDeleteInTeamPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FKC_TeamPlayer_Player",
                table: "TeamPlayers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "36626fd1-9888-4152-a2b5-e2c83e33a9d6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "561ff884-04e3-4691-b26e-6a787ce96046");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af251c60-a36f-4e46-8456-0c11c873224d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5ab32866-2379-427c-9116-19f92b3f3737", "9b2a8c3b-de03-4b75-beff-9746405e74bb", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d7210450-0934-4000-843d-81fbe9dbaa6f", "dce62311-0140-4026-aa43-9b3aadbf5966", "RegularUser", "REGULARUSER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1148a1fa-adb2-4483-a0bf-0e248578ba81", "d66d58c6-24dc-4fb2-be0b-96c8ffa426af", "PremiumUser", "PREMIUMUSER" });

            migrationBuilder.AddForeignKey(
                name: "FKC_TeamPlayer_Player",
                table: "TeamPlayers",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FKC_TeamPlayer_Player",
                table: "TeamPlayers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1148a1fa-adb2-4483-a0bf-0e248578ba81");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ab32866-2379-427c-9116-19f92b3f3737");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d7210450-0934-4000-843d-81fbe9dbaa6f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "36626fd1-9888-4152-a2b5-e2c83e33a9d6", "83649b85-286a-4708-bb60-d73a7e6d0923", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "561ff884-04e3-4691-b26e-6a787ce96046", "531957b7-f720-49df-969a-5d24e234de60", "RegularUser", "REGULARUSER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "af251c60-a36f-4e46-8456-0c11c873224d", "15eeba01-f802-4d91-88b0-49ef67b16bbd", "PremiumUser", "PREMIUMUSER" });

            migrationBuilder.AddForeignKey(
                name: "FKC_TeamPlayer_Player",
                table: "TeamPlayers",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }
    }
}
