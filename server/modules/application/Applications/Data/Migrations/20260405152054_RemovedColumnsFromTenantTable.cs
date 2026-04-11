using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Applications.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedColumnsFromTenantTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                schema: "applications",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "applications",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "applications",
                table: "Tenants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "applications",
                table: "Tenants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "applications",
                table: "Tenants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "applications",
                table: "Tenants",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
