using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Grad.Data.Migrations
{
    public partial class statmigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StateDescr",
                table: "States",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "States",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Checked",
                table: "Notes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Fixed",
                table: "Notes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    StatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StatusName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.StatusId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_States_StatusId",
                table: "States",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_States_Status_StatusId",
                table: "States",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_States_Status_StatusId",
                table: "States");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropIndex(
                name: "IX_States_StatusId",
                table: "States");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "States");

            migrationBuilder.DropColumn(
                name: "Checked",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Fixed",
                table: "Notes");

            migrationBuilder.AlterColumn<string>(
                name: "StateDescr",
                table: "States",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
