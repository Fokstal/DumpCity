using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DumpCity.Migrations
{
    /// <inheritdoc />
    public partial class AddShortDescToProductTable_and_UpdateDescInProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Product",
                newName: "ShortDesc");

            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desc",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "ShortDesc",
                table: "Product",
                newName: "Description");
        }
    }
}
