using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingCapacityManagement.Migrations
{
    public partial class TrainingDateSelection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateSelection",
                table: "Training",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateSelection",
                table: "Training");
        }
    }
}
