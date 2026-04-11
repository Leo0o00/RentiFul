using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Applications.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedApprovedApplicationSagaDataTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UpdateApplicationSagaData",
                schema: "applications",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentState = table.Column<string>(type: "text", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantCognitoId = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uuid", nullable: false),
                    LeaseCreated = table.Column<bool>(type: "boolean", nullable: false),
                    LeaseLinkedToApplication = table.Column<bool>(type: "boolean", nullable: false),
                    TenantOwnedPropertyAdded = table.Column<bool>(type: "boolean", nullable: false),
                    ApproveApplicationProcessCompleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateApplicationSagaData", x => x.CorrelationId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpdateApplicationSagaData",
                schema: "applications");
        }
    }
}
