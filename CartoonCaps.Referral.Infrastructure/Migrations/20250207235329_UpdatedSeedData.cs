using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CartoonCaps.Referral.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "ReferralCode" },
                values: new object[,]
                {
                    { 3, "Bob Johnson", "DEF456" },
                    { 4, "Alice Williams", "GHI789" },
                    { 5, "Tom Brown", "JKL123" },
                    { 6, "Emily Davis", "MNO456" },
                    { 7, "David Wilson", "PQR789" },
                    { 8, "Olivia Taylor", "STU123" },
                    { 9, "Jack Anderson", "VWX456" },
                    { 10, "Grace Thomas", "YZA789" }
                });

            migrationBuilder.InsertData(
                table: "ReferralRecords",
                columns: new[] { "Id", "RefereeId", "ReferralStatus", "ReferrerId" },
                values: new object[,]
                {
                    { 2, 3, "Pending", 1 },
                    { 3, 4, "Pending", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ReferralRecords",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ReferralRecords",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
