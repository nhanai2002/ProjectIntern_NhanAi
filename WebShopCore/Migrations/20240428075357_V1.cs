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
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 28, 14, 53, 57, 29, DateTimeKind.Local).AddTicks(3319), new DateTime(2024, 4, 28, 14, 53, 57, 29, DateTimeKind.Local).AddTicks(3301) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 28, 14, 53, 57, 29, DateTimeKind.Local).AddTicks(3442), new DateTime(2024, 4, 28, 14, 53, 57, 29, DateTimeKind.Local).AddTicks(3442) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 27, 14, 42, 26, 256, DateTimeKind.Local).AddTicks(9511), new DateTime(2024, 4, 27, 14, 42, 26, 256, DateTimeKind.Local).AddTicks(9491) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 27, 14, 42, 26, 256, DateTimeKind.Local).AddTicks(9562), new DateTime(2024, 4, 27, 14, 42, 26, 256, DateTimeKind.Local).AddTicks(9562) });
        }
    }
}
