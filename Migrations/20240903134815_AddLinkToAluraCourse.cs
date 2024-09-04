using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrawlerAlura.Migrations
{
    /// <inheritdoc />
    public partial class AddLinkToAluraCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "AluraCourses",
                type: "TEXT",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "AluraCourses");
        }
    }
}
