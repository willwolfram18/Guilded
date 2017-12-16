using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Guilded.Migrations.SqlServer.Migrations
{
    public partial class SoftThreadAndReplyDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_ApplicationRoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoleClaims_ApplicationRoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropColumn(
                name: "ApplicationRoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Threads",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Replies",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Replies");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationRoleId",
                table: "AspNetRoleClaims",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_ApplicationRoleId",
                table: "AspNetRoleClaims",
                column: "ApplicationRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_ApplicationRoleId",
                table: "AspNetRoleClaims",
                column: "ApplicationRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
