using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingCapacityManagement.Migrations
{
    public partial class FixingSportinTraining : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Training_Sport_SportId",
                table: "Training");

            migrationBuilder.AlterColumn<int>(
                name: "SportId",
                table: "Training",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Training_Sport_SportId",
                table: "Training",
                column: "SportId",
                principalTable: "Sport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Training_Sport_SportId",
                table: "Training");

            migrationBuilder.AlterColumn<int>(
                name: "SportId",
                table: "Training",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Training_Sport_SportId",
                table: "Training",
                column: "SportId",
                principalTable: "Sport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
