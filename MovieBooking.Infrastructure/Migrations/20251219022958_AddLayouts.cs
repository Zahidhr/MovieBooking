using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLayouts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Layouts",
                columns: table => new
                {
                    LayoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RowCount = table.Column<int>(type: "int", nullable: false),
                    SeatsPerRow = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layouts", x => x.LayoutId);
                });

            migrationBuilder.CreateTable(
                name: "LayoutSeatTypes",
                columns: table => new
                {
                    LayoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowNumber = table.Column<int>(type: "int", nullable: false),
                    SeatNumber = table.Column<int>(type: "int", nullable: false),
                    SeatType = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LayoutSeatTypes", x => new { x.LayoutId, x.RowNumber, x.SeatNumber });
                    table.ForeignKey(
                        name: "FK_LayoutSeatTypes_Layouts_LayoutId",
                        column: x => x.LayoutId,
                        principalTable: "Layouts",
                        principalColumn: "LayoutId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LayoutSeatTypes");

            migrationBuilder.DropTable(
                name: "Layouts");
        }
    }
}
