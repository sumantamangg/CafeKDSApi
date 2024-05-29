using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeKDSApi.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderTabletoDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DrinkType = table.Column<int>(type: "int", nullable: false),
                    DrinkSize = table.Column<int>(type: "int", nullable: false),
                    MilkChoice = table.Column<int>(type: "int", nullable: false),
                    Shots = table.Column<int>(type: "int", nullable: false),
                    OrderTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimerEndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
