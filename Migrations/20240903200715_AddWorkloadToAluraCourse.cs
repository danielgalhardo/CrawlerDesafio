using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrawlerAlura.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkloadToAluraCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Workload",
                table: "AluraCourses",
                type: "TEXT",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Workload",
                table: "AluraCourses");
        }
    }
}
