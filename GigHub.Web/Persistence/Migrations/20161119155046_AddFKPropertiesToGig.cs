using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GigHub.Web.Persistence.Migrations
{
    public partial class AddFKPropertiesToGig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreatePostgresExtension("uuid-ossp");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Gigs",
                maxLength: 255,
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPostgresExtension("uuid-ossp");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Gigs",
                nullable: false)
                .Annotation("Npgsql:ValueGeneratedOnAdd", true);
        }
    }
}
