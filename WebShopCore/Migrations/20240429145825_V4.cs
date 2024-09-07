using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopCore.Migrations
{
    /// <inheritdoc />
    public partial class V4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Seen",
                table: "notifications",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SendAt",
                table: "notifications",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 29, 21, 58, 24, 884, DateTimeKind.Local).AddTicks(9820), new DateTime(2024, 4, 29, 21, 58, 24, 884, DateTimeKind.Local).AddTicks(9799) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 29, 21, 58, 24, 884, DateTimeKind.Local).AddTicks(9983), new DateTime(2024, 4, 29, 21, 58, 24, 884, DateTimeKind.Local).AddTicks(9983) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seen",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "SendAt",
                table: "notifications");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 28, 15, 44, 49, 484, DateTimeKind.Local).AddTicks(5079), new DateTime(2024, 4, 28, 15, 44, 49, 484, DateTimeKind.Local).AddTicks(5063) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 28, 15, 44, 49, 484, DateTimeKind.Local).AddTicks(5131), new DateTime(2024, 4, 28, 15, 44, 49, 484, DateTimeKind.Local).AddTicks(5130) });
        }
    }
}
