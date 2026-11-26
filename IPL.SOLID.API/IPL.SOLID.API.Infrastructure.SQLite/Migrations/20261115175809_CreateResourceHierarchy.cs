using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IPL.SOLID.API.Infrastructure.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class CreateResourceHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_DataRecord_ContentType",
                table: "DataRecord");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Expiration",
                table: "Subscription",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Expiration",
                table: "DataRecord",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationId",
                table: "Channel",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Channel",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Applications",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddCheckConstraint(
                name: "CK_DataRecord_ContentType",
                table: "DataRecord",
                sql: "ContentType IN ('application/xml', 'application/json', 'application/plaintext', 'text/plain')");

            migrationBuilder.CreateIndex(
                name: "IX_Channel_ApplicationId",
                table: "Channel",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Applications_ApplicationId",
                table: "Channel",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Applications_ApplicationId",
                table: "Channel");

            migrationBuilder.DropCheckConstraint(
                name: "CK_DataRecord_ContentType",
                table: "DataRecord");

            migrationBuilder.DropIndex(
                name: "IX_Channel_ApplicationId",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "Expiration",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "Expiration",
                table: "DataRecord");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Applications");

            migrationBuilder.AddCheckConstraint(
                name: "CK_DataRecord_ContentType",
                table: "DataRecord",
                sql: "ContentType IN ('application/xml', 'application/json', 'application/plaintext')");
        }
    }
}
