using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopCore.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "orders",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 3, 28, 13, 23, 9, 708, DateTimeKind.Local).AddTicks(3001), new DateTime(2024, 3, 28, 13, 23, 9, 708, DateTimeKind.Local).AddTicks(2986) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 3, 28, 13, 23, 9, 708, DateTimeKind.Local).AddTicks(3048), new DateTime(2024, 3, 28, 13, 23, 9, 708, DateTimeKind.Local).AddTicks(3048) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "orders");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 3, 14, 21, 26, 51, 280, DateTimeKind.Local).AddTicks(2643), new DateTime(2024, 3, 14, 21, 26, 51, 280, DateTimeKind.Local).AddTicks(2623) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 3, 14, 21, 26, 51, 280, DateTimeKind.Local).AddTicks(2688), new DateTime(2024, 3, 14, 21, 26, 51, 280, DateTimeKind.Local).AddTicks(2688) });
        }
    }
}
