using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Grad.Migrations
{
    public partial class immigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Pictures");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Pictures",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Pictures");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Pictures",
                nullable: true);
        }
    }
}
