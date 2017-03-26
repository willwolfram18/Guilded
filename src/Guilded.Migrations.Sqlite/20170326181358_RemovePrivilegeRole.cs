using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Guilded.Migrations.Sqlite
{
    public partial class RemovePrivilegeRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRolePrivileges");

            migrationBuilder.DropTable(
                name: "AspNetPrivileges");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_Name",
                table: "AspNetRoles",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_Name",
                table: "AspNetRoles");

            migrationBuilder.CreateTable(
                name: "AspNetPrivileges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetPrivileges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRolePrivileges",
                columns: table => new
                {
                    RoleId = table.Column<string>(nullable: false),
                    PrivilegeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRolePrivileges", x => new { x.RoleId, x.PrivilegeId });
                    table.ForeignKey(
                        name: "FK_AspNetRolePrivileges_AspNetPrivileges_PrivilegeId",
                        column: x => x.PrivilegeId,
                        principalTable: "AspNetPrivileges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetRolePrivileges_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRolePrivileges_PrivilegeId",
                table: "AspNetRolePrivileges",
                column: "PrivilegeId");
        }
    }
}
