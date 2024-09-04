using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrawlerAlura.Migrations
{
    /// <inheritdoc />
    public partial class AddInstructorToAluraCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Instructor",
                table: "AluraCourses",
                type: "TEXT",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Instructor",
                table: "AluraCourses");
        }
    }
}
