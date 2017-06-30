using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Guilded.Migrations.SqlServer
{
    public partial class Added_Thread_To_Forum_Relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ForumId",
                table: "Threads",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Threads_ForumId",
                table: "Threads",
                column: "ForumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Forums_ForumId",
                table: "Threads",
                column: "ForumId",
                principalTable: "Forums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Forums_ForumId",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Threads_ForumId",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "ForumId",
                table: "Threads");
        }
    }
}
