using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Properties.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedManagerTableRollbackManagerIdAndAddedManagerForeignKeyToPropertyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Managers_ManagerAssignedManagerCognitoId",
                schema: "properties",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "ManagerAssignedManagerCognitoId",
                schema: "properties",
                table: "Properties",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_ManagerAssignedManagerCognitoId",
                schema: "properties",
                table: "Properties",
                newName: "IX_Properties_ManagerId");

            migrationBuilder.RenameColumn(
                name: "ManagerCognitoId",
                schema: "properties",
                table: "Managers",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Managers_ManagerId",
                schema: "properties",
                table: "Properties",
                column: "ManagerId",
                principalSchema: "properties",
                principalTable: "Managers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Managers_ManagerId",
                schema: "properties",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                schema: "properties",
                table: "Properties",
                newName: "ManagerAssignedManagerCognitoId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_ManagerId",
                schema: "properties",
                table: "Properties",
                newName: "IX_Properties_ManagerAssignedManagerCognitoId");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "properties",
                table: "Managers",
                newName: "ManagerCognitoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Managers_ManagerAssignedManagerCognitoId",
                schema: "properties",
                table: "Properties",
                column: "ManagerAssignedManagerCognitoId",
                principalSchema: "properties",
                principalTable: "Managers",
                principalColumn: "ManagerCognitoId");
        }
    }
}
