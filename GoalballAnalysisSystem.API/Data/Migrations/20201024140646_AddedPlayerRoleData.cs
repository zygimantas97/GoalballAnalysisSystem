using Microsoft.EntityFrameworkCore.Migrations;

namespace GoalballAnalysisSystem.API.Data.Migrations
{
    public partial class AddedPlayerRoleData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                values: new object[,]
                {
                    { "36626fd1-9888-4152-a2b5-e2c83e33a9d6", "83649b85-286a-4708-bb60-d73a7e6d0923", "Administrator", "ADMINISTRATOR" },
                    { "561ff884-04e3-4691-b26e-6a787ce96046", "531957b7-f720-49df-969a-5d24e234de60", "RegularUser", "REGULARUSER" },
                    { "af251c60-a36f-4e46-8456-0c11c873224d", "15eeba01-f802-4d91-88b0-49ef67b16bbd", "PremiumUser", "PREMIUMUSER" }
                });

            migrationBuilder.InsertData(
                table: "PlayerRoles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "LeftStriker" },
                    { 2L, "RightStriker" },
                    { 3L, "Center" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DeleteData(
                table: "PlayerRoles",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "PlayerRoles",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "PlayerRoles",
                keyColumn: "Id",
                keyValue: 3L);

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
        }
    }
}
