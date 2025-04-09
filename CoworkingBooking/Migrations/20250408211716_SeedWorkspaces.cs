using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoworkingBooking.Migrations
{
    /// <inheritdoc />
    public partial class SeedWorkspaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Workspaces",
                columns: new[] { "Id", "IsAvailable", "Location", "Name" },
                values: new object[,]
                {
                    { 4, true, "First Floor", "Room C" },
                    { 5, true, "Second Floor", "Room D" },
                    { 6, true, "Second Floor", "Room E" },
                    { 7, true, "Third Floor", "Room F" },
                    { 8, true, "Third Floor", "Room G" },
                    { 9, true, "First Floor", "Focus Booth 1" },
                    { 10, true, "First Floor", "Focus Booth 2" },
                    { 11, true, "Second Floor", "Open Desk 1" },
                    { 12, true, "Second Floor", "Open Desk 2" },
                    { 13, true, "Third Floor", "Quiet Zone" },
                    { 14, true, "Fourth Floor", "Hot Desk 1" },
                    { 15, true, "Fourth Floor", "Hot Desk 2" },
                    { 16, true, "Fourth Floor", "Hot Desk 3" },
                    { 17, true, "First Floor", "Solo Cabin 1" },
                    { 18, true, "First Floor", "Solo Cabin 2" },
                    { 19, true, "Second Floor", "Team Table 1" },
                    { 20, true, "Second Floor", "Team Table 2" },
                    { 21, true, "Third Floor", "Conference Spot" },
                    { 22, true, "Third Floor", "Brainstorm Zone" },
                    { 23, true, "Top Floor", "Meeting Box" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: 23);
        }
    }
}
