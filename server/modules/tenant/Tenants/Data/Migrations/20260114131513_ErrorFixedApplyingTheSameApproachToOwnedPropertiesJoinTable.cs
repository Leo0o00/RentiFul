using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tenants.Data.Migrations
{
    /// <inheritdoc />
    public partial class ErrorFixedApplyingTheSameApproachToOwnedPropertiesJoinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OwnedProperties",
                schema: "tenants",
                table: "OwnedProperties");

            migrationBuilder.DropIndex(
                name: "IX_OwnedProperties_TenantId",
                schema: "tenants",
                table: "OwnedProperties");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OwnedProperties",
                schema: "tenants",
                table: "OwnedProperties",
                columns: new[] { "TenantId", "PropertyId" });

            migrationBuilder.CreateIndex(
                name: "IX_OwnedProperties_PropertyId",
                schema: "tenants",
                table: "OwnedProperties",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnedProperties_TenantId_PropertyId",
                schema: "tenants",
                table: "OwnedProperties",
                columns: new[] { "TenantId", "PropertyId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OwnedProperties",
                schema: "tenants",
                table: "OwnedProperties");

            migrationBuilder.DropIndex(
                name: "IX_OwnedProperties_PropertyId",
                schema: "tenants",
                table: "OwnedProperties");

            migrationBuilder.DropIndex(
                name: "IX_OwnedProperties_TenantId_PropertyId",
                schema: "tenants",
                table: "OwnedProperties");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OwnedProperties",
                schema: "tenants",
                table: "OwnedProperties",
                columns: new[] { "PropertyId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_OwnedProperties_TenantId",
                schema: "tenants",
                table: "OwnedProperties",
                column: "TenantId");
        }
    }
}
