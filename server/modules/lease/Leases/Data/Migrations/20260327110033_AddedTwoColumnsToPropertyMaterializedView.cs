using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leases.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedTwoColumnsToPropertyMaterializedView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PricePerMonth",
                schema: "leases",
                table: "Properties",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SecurityDeposit",
                schema: "leases",
                table: "Properties",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerMonth",
                schema: "leases",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "SecurityDeposit",
                schema: "leases",
                table: "Properties");
        }
    }
}
