using Microsoft.EntityFrameworkCore.Migrations;

namespace UrlShortenerDAL.EF.Migrations
{
    public partial class OwnerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Original",
                table: "Links",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Links",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Links");

            migrationBuilder.AlterColumn<string>(
                name: "Original",
                table: "Links",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
