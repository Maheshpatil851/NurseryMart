using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NurseryMart.Migrations
{
    /// <inheritdoc />
    public partial class onetomanyrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authorize_Trail_TrailId",
                table: "Authorize");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Trail",
                newName: "TrailId");

            migrationBuilder.AlterColumn<string>(
                name: "mobile",
                table: "Authorize",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrailId",
                table: "Authorize",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorize_Trail_TrailId",
                table: "Authorize",
                column: "TrailId",
                principalTable: "Trail",
                principalColumn: "TrailId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authorize_Trail_TrailId",
                table: "Authorize");

            migrationBuilder.RenameColumn(
                name: "TrailId",
                table: "Trail",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "mobile",
                table: "Authorize",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "TrailId",
                table: "Authorize",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Authorize_Trail_TrailId",
                table: "Authorize",
                column: "TrailId",
                principalTable: "Trail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
