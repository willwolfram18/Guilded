using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Guilded.Migrations.SqlServer
{
    public partial class Included_Slug_On_Forum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubTitle",
                table: "Forums",
                newName: "Subtitle");

            migrationBuilder.AlterColumn<Guid>(
                name: "Version",
                table: "ForumSections",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Version",
                table: "Forums",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Forums",
                maxLength: 35,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Forums");

            migrationBuilder.RenameColumn(
                name: "Subtitle",
                table: "Forums",
                newName: "SubTitle");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Version",
                table: "ForumSections",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<byte[]>(
                name: "Version",
                table: "Forums",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(Guid));
        }
    }
}
