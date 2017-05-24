using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Guilded.Migrations.SqlServer
{
    public partial class EnableUserProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_AspNetRoles_ParentRoleId",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_ParentRoleId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "ParentRoleId",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<DateTime>(
                name: "EnabledAfter",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnabledAfter",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ParentRoleId",
                table: "AspNetRoles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_ParentRoleId",
                table: "AspNetRoles",
                column: "ParentRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_AspNetRoles_ParentRoleId",
                table: "AspNetRoles",
                column: "ParentRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
