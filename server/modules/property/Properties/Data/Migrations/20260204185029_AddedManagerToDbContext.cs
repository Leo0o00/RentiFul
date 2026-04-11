using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Properties.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedManagerToDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Manager_ManagerAssignedId",
                schema: "properties",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Manager",
                schema: "properties",
                table: "Manager");

            migrationBuilder.RenameTable(
                name: "Manager",
                schema: "properties",
                newName: "Managers",
                newSchema: "properties");

            migrationBuilder.AlterColumn<double>(
                name: "SquareFeet",
                schema: "properties",
                table: "Properties",
                type: "double precision",
                precision: 7,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Managers",
                schema: "properties",
                table: "Managers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Managers_ManagerAssignedId",
                schema: "properties",
                table: "Properties",
                column: "ManagerAssignedId",
                principalSchema: "properties",
                principalTable: "Managers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Managers_ManagerAssignedId",
                schema: "properties",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Managers",
                schema: "properties",
                table: "Managers");

            migrationBuilder.RenameTable(
                name: "Managers",
                schema: "properties",
                newName: "Manager",
                newSchema: "properties");

            migrationBuilder.AlterColumn<int>(
                name: "SquareFeet",
                schema: "properties",
                table: "Properties",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldPrecision: 7,
                oldScale: 2);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Manager",
                schema: "properties",
                table: "Manager",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Manager_ManagerAssignedId",
                schema: "properties",
                table: "Properties",
                column: "ManagerAssignedId",
                principalSchema: "properties",
                principalTable: "Manager",
                principalColumn: "Id");
        }
    }
}
