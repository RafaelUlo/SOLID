using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IPL.SOLID.API.Infrastructure.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ResourceName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Channel",
                columns: table => new
                {
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ResourceName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataRecord",
                columns: table => new
                {
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChannelId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    ResourceName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataRecord", x => x.Id);
                    table.CheckConstraint("CK_DataRecord_ContentType", "ContentType IN ('application/xml', 'application/json', 'application/plaintext')");
                    table.ForeignKey(
                        name: "FK_DataRecord_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChannelId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Evt = table.Column<int>(type: "INTEGER", nullable: false),
                    Endpoint = table.Column<string>(type: "TEXT", nullable: false),
                    ResourceName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscription_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ResourceName",
                table: "Applications",
                column: "ResourceName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channel_ResourceName",
                table: "Channel",
                column: "ResourceName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataRecord_ChannelId",
                table: "DataRecord",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_DataRecord_ResourceName",
                table: "DataRecord",
                column: "ResourceName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_ChannelId",
                table: "Subscription",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_ResourceName",
                table: "Subscription",
                column: "ResourceName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "DataRecord");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "Channel");
        }
    }
}
