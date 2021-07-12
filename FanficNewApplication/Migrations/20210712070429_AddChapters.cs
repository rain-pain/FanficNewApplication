using Microsoft.EntityFrameworkCore.Migrations;

namespace FanficNewApplication.Migrations
{
    public partial class AddChapters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Fanfic");

            migrationBuilder.CreateTable(
                name: "Chapter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "text", nullable: true),
                    FanficId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapter_Fanfic_FanficId",
                        column: x => x.FanficId,
                        principalTable: "Fanfic",
                        principalColumn: "FanficId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_FanficId",
                table: "Chapter",
                column: "FanficId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chapter");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Fanfic",
                type: "text",
                nullable: true);
        }
    }
}
