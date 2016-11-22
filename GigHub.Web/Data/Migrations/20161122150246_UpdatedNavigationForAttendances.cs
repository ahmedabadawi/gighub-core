using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GigHub.Web.Data.Migrations
{
    public partial class UpdatedNavigationForAttendances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Gigs_GigId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Followings_AspNetUsers_FollowerId",
                table: "Followings");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Gigs_GigId",
                table: "Attendances",
                column: "GigId",
                principalTable: "Gigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Followings_AspNetUsers_FollowerId",
                table: "Followings",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Gigs_GigId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Followings_AspNetUsers_FollowerId",
                table: "Followings");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Gigs_GigId",
                table: "Attendances",
                column: "GigId",
                principalTable: "Gigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Followings_AspNetUsers_FollowerId",
                table: "Followings",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
