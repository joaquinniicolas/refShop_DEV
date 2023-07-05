using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace refShop_DEV.Migrations.MigrationsFolder
{
    public partial class UpdatedDBTurnos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_UserRoles_UserRoleId",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionsId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_UserRoles_UserRoleId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_PermissionsId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_UserRoleId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_UserRoleId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "PermissionsId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "UserRoleId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "UserRoleId",
                table: "Permissions");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Permissions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.CreateTable(
                name: "Turnos",
                columns: table => new
                {
                    IDTurno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDEmpleado = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreadoPor = table.Column<int>(type: "int", nullable: false),
                    Dias_Laborales = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.IDTurno);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Turnos");

            migrationBuilder.AddColumn<int>(
                name: "PermissionsId",
                table: "RolePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserRoleId",
                table: "RolePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Permissions",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "UserRoleId",
                table: "Permissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionsId",
                table: "RolePermissions",
                column: "PermissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_UserRoleId",
                table: "RolePermissions",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UserRoleId",
                table: "Permissions",
                column: "UserRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_UserRoles_UserRoleId",
                table: "Permissions",
                column: "UserRoleId",
                principalTable: "UserRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionsId",
                table: "RolePermissions",
                column: "PermissionsId",
                principalTable: "Permissions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_UserRoles_UserRoleId",
                table: "RolePermissions",
                column: "UserRoleId",
                principalTable: "UserRoles",
                principalColumn: "Id");
        }
    }
}
