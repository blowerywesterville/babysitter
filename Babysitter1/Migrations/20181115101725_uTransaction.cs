using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Babysitter1.Migrations
{
    public partial class uTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Transaction");

            migrationBuilder.AddColumn<int>(
                name: "EarlyHours",
                table: "Transaction",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EveningHours",
                table: "Transaction",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LateHours",
                table: "Transaction",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EarlyHours",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "EveningHours",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "LateHours",
                table: "Transaction");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Transaction",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Transaction",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
