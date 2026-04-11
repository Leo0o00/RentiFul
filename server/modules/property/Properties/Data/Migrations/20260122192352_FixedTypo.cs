using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Properties.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPetsAlowwed",
                schema: "properties",
                table: "Properties",
                newName: "IsPetsAllowed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPetsAllowed",
                schema: "properties",
                table: "Properties",
                newName: "IsPetsAlowwed");
        }
    }
}
