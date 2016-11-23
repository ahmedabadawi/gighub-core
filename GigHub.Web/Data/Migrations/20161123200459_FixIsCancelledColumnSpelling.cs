using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GigHub.Web.Data.Migrations
{
    public partial class FixIsCancelledColumnSpelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Gigs");

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "Gigs",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "Gigs");

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Gigs",
                nullable: false,
                defaultValue: false);
        }
    }
}
