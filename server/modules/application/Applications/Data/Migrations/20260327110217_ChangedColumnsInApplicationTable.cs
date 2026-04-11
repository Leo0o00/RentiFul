using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Applications.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedColumnsInApplicationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                schema: "applications",
                table: "Applications",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                schema: "applications",
                table: "Applications",
                newName: "SubmittedAt");

            migrationBuilder.RenameColumn(
                name: "ApplicationDate",
                schema: "applications",
                table: "Applications",
                newName: "StatusChangedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "applications",
                table: "Applications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "applications",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "applications",
                table: "Applications",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "SubmittedAt",
                schema: "applications",
                table: "Applications",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "StatusChangedAt",
                schema: "applications",
                table: "Applications",
                newName: "ApplicationDate");
        }
    }
}
