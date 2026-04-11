using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Properties.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedManagerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Managers_ManagerAssignedId",
                schema: "properties",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "ManagerAssignedId",
                schema: "properties",
                table: "Properties",
                newName: "ManagerAssignedManagerCognitoId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_ManagerAssignedId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Managers_ManagerAssignedManagerCognitoId",
                schema: "properties",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "ManagerAssignedManagerCognitoId",
                schema: "properties",
                table: "Properties",
                newName: "ManagerAssignedId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_ManagerAssignedManagerCognitoId",
                schema: "properties",
                table: "Properties",
                newName: "IX_Properties_ManagerAssignedId");

            migrationBuilder.RenameColumn(
                name: "ManagerCognitoId",
                schema: "properties",
                table: "Managers",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Managers_ManagerAssignedId",
                schema: "properties",
                table: "Properties",
                column: "ManagerAssignedId",
                principalSchema: "properties",
                principalTable: "Managers",
                principalColumn: "Id");
        }
    }
}
