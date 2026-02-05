using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NguyenDucManh_SE1884_A01_BE.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToNewsArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "NewsArticle",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "NewsArticle");
        }
    }
}
