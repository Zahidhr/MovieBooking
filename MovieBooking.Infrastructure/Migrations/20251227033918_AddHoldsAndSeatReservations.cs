using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHoldsAndSeatReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Holds",
                columns: table => new
                {
                    HoldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScreeningId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holds", x => x.HoldId);
                });

            migrationBuilder.CreateTable(
                name: "SeatReservations",
                columns: table => new
                {
                    ScreeningId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowNumber = table.Column<int>(type: "int", nullable: false),
                    SeatNumber = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    HoldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatReservations", x => new { x.ScreeningId, x.RowNumber, x.SeatNumber });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Holds_ExpiresAt",
                table: "Holds",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_Holds_ScreeningId_Status",
                table: "Holds",
                columns: new[] { "ScreeningId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_SeatReservations_ExpiresAt",
                table: "SeatReservations",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_SeatReservations_ScreeningId_Status",
                table: "SeatReservations",
                columns: new[] { "ScreeningId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Holds");

            migrationBuilder.DropTable(
                name: "SeatReservations");
        }
    }
}
