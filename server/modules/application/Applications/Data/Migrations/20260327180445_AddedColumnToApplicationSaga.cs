using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Applications.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnToApplicationSaga : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LeaseId",
                schema: "applications",
                table: "UpdateApplicationSagaData",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaseId",
                schema: "applications",
                table: "UpdateApplicationSagaData");
        }
    }
}
