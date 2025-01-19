using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NurseryMart.Migrations
{
    /// <inheritdoc />
    public partial class addedsalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "salt",
                table: "Authorize",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "salt",
                table: "Authorize");
        }
    }
}
