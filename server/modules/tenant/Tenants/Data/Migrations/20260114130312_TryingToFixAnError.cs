using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tenants.Data.Migrations
{
    /// <inheritdoc />
    public partial class TryingToFixAnError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteProperties",
                schema: "tenants",
                table: "FavoriteProperties");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteProperties_TenantId",
                schema: "tenants",
                table: "FavoriteProperties");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteProperties",
                schema: "tenants",
                table: "FavoriteProperties",
                columns: new[] { "TenantId", "PropertyId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Email",
                schema: "tenants",
                table: "Tenants",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProperties_PropertyId",
                schema: "tenants",
                table: "FavoriteProperties",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProperties_TenantId_PropertyId",
                schema: "tenants",
                table: "FavoriteProperties",
                columns: new[] { "TenantId", "PropertyId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tenants_Email",
                schema: "tenants",
                table: "Tenants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteProperties",
                schema: "tenants",
                table: "FavoriteProperties");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteProperties_PropertyId",
                schema: "tenants",
                table: "FavoriteProperties");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteProperties_TenantId_PropertyId",
                schema: "tenants",
                table: "FavoriteProperties");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteProperties",
                schema: "tenants",
                table: "FavoriteProperties",
                columns: new[] { "PropertyId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProperties_TenantId",
                schema: "tenants",
                table: "FavoriteProperties",
                column: "TenantId");
        }
    }
}
