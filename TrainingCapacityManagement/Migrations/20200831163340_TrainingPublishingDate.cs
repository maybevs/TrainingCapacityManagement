using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingCapacityManagement.Migrations
{
    public partial class TrainingPublishingDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Training_Sport_SportId",
                table: "Training");

            migrationBuilder.AlterColumn<int>(
                name: "SportId",
                table: "Training",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishingDate",
                table: "Training",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Training_Sport_SportId",
                table: "Training",
                column: "SportId",
                principalTable: "Sport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Training_Sport_SportId",
                table: "Training");

            migrationBuilder.DropColumn(
                name: "PublishingDate",
                table: "Training");

           migrationBuilder.AlterColumn<int>(
                name: "SportId",
                table: "Training",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Training_Sport_SportId",
                table: "Training",
                column: "SportId",
                principalTable: "Sport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
