using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tenants.Data.Migrations
{
    /// <inheritdoc />
    public partial class TenantSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "tenants");

            migrationBuilder.CreateTable(
                name: "Properties",
                schema: "tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                schema: "tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CognitoId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                    table.UniqueConstraint("AK_Tenants_CognitoId", x => x.CognitoId);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteProperties",
                schema: "tenants",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteProperties", x => new { x.PropertyId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_FavoriteProperties_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "tenants",
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteProperties_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "tenants",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OwnedProperties",
                schema: "tenants",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnedProperties", x => new { x.PropertyId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_OwnedProperties_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "tenants",
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OwnedProperties_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "tenants",
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProperties_TenantId",
                schema: "tenants",
                table: "FavoriteProperties",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnedProperties_TenantId",
                schema: "tenants",
                table: "OwnedProperties",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteProperties",
                schema: "tenants");

            migrationBuilder.DropTable(
                name: "OwnedProperties",
                schema: "tenants");

            migrationBuilder.DropTable(
                name: "Properties",
                schema: "tenants");

            migrationBuilder.DropTable(
                name: "Tenants",
                schema: "tenants");
        }
    }
}
