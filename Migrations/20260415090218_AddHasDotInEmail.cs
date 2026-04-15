using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace onilne_service.Migrations
{
    /// <inheritdoc />
    public partial class AddHasDotInEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HasDotInEmail",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasDotInEmail",
                table: "AspNetUsers");
        }
    }
}
