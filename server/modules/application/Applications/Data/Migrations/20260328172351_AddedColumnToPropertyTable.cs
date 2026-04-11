using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Applications.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnToPropertyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId",
                schema: "applications",
                table: "Properties",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagerId",
                schema: "applications",
                table: "Properties");
        }
    }
}
