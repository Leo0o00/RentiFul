using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Properties.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "properties");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "Locations",
                schema: "properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Coordinates = table.Column<Point>(type: "geography (Point, 4326)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manager",
                schema: "properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                schema: "properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    PricePerMonth = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    SecurityDeposit = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    ApplicationFee = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false),
                    Amenities = table.Column<string>(type: "text", nullable: false),
                    Highlights = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    IsPetsAlowwed = table.Column<bool>(type: "boolean", nullable: false),
                    IsParkingIncluded = table.Column<bool>(type: "boolean", nullable: false),
                    Beds = table.Column<int>(type: "integer", nullable: false),
                    Baths = table.Column<double>(type: "double precision", precision: 2, scale: 1, nullable: false),
                    SquareFeet = table.Column<int>(type: "integer", nullable: false),
                    PostedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AverageRating = table.Column<double>(type: "double precision", precision: 1, scale: 1, nullable: false),
                    NumberOfReviews = table.Column<int>(type: "integer", nullable: true),
                    PropertyLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManagerAssignedId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_Locations_PropertyLocationId",
                        column: x => x.PropertyLocationId,
                        principalSchema: "properties",
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Properties_Manager_ManagerAssignedId",
                        column: x => x.ManagerAssignedId,
                        principalSchema: "properties",
                        principalTable: "Manager",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PropertyMedia",
                schema: "properties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PropertyId = table.Column<Guid>(type: "uuid", nullable: false),
                    PhotoKey = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyMedia_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "properties",
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_ManagerAssignedId",
                schema: "properties",
                table: "Properties",
                column: "ManagerAssignedId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyLocationId",
                schema: "properties",
                table: "Properties",
                column: "PropertyLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyMedia_PropertyId",
                schema: "properties",
                table: "PropertyMedia",
                column: "PropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyMedia",
                schema: "properties");

            migrationBuilder.DropTable(
                name: "Properties",
                schema: "properties");

            migrationBuilder.DropTable(
                name: "Locations",
                schema: "properties");

            migrationBuilder.DropTable(
                name: "Manager",
                schema: "properties");
        }
    }
}
