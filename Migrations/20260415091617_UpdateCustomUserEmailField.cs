using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace onilne_service.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomUserEmailField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasDotInEmail",
                table: "AspNetUsers",
                newName: "NormalizedCustomEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NormalizedCustomEmail",
                table: "AspNetUsers",
                newName: "HasDotInEmail");
        }
    }
}
