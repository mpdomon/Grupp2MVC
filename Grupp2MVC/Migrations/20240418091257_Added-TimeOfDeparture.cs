using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grupp2MVC.Migrations
{
    /// <inheritdoc />
    public partial class AddedTimeOfDeparture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeOfDeparture",
                table: "Vehicle",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOfDeparture",
                table: "Vehicle");
        }
    }
}
