using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Grad.Migrations
{
    public partial class picdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descr",
                table: "Pictures");

            migrationBuilder.AddColumn<DateTime>(
                name: "uploaddate",
                table: "Pictures",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "uploaddate",
                table: "Pictures");

            migrationBuilder.AddColumn<string>(
                name: "Descr",
                table: "Pictures",
                nullable: true);
        }
    }
}
