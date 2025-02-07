using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CartoonCaps.Referral.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ReferralCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReferralRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RefereeId = table.Column<int>(type: "integer", nullable: false),
                    ReferrerId = table.Column<int>(type: "integer", nullable: false),
                    ReferralStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferralRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferralRecords_Users_RefereeId",
                        column: x => x.RefereeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReferralRecords_Users_ReferrerId",
                        column: x => x.ReferrerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "ReferralCode" },
                values: new object[,]
                {
                    { 1, "John Doe", "ABC123" },
                    { 2, "Jane Smith", "XYZ789" }
                });

            migrationBuilder.InsertData(
                table: "ReferralRecords",
                columns: new[] { "Id", "RefereeId", "ReferralStatus", "ReferrerId" },
                values: new object[] { 1, 2, "Pending", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_ReferralRecords_RefereeId",
                table: "ReferralRecords",
                column: "RefereeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReferralRecords_RefereeId_ReferrerId",
                table: "ReferralRecords",
                columns: new[] { "RefereeId", "ReferrerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReferralRecords_ReferrerId",
                table: "ReferralRecords",
                column: "ReferrerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReferralCode",
                table: "Users",
                column: "ReferralCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferralRecords");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
