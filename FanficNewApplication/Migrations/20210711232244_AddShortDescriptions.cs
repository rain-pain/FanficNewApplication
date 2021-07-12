using Microsoft.EntityFrameworkCore.Migrations;

namespace FanficNewApplication.Migrations
{
    public partial class AddShortDescriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Fanfic",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Fandom",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Fanfic");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Fandom");
        }
    }
}
