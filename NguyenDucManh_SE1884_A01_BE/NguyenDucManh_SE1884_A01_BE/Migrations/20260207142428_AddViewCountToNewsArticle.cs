using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NguyenDucManh_SE1884_A01_BE.Migrations
{
    /// <inheritdoc />
    public partial class AddViewCountToNewsArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "NewsArticle",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "NewsArticle");
        }
    }
}
