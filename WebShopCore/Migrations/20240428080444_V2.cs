using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopCore.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_notifications_NotificationId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "NotificationId",
                table: "User",
                newName: "NotificationIdNoti");

            migrationBuilder.RenameIndex(
                name: "IX_User_NotificationId",
                table: "User",
                newName: "IX_User_NotificationIdNoti");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "notifications",
                newName: "IdNoti");

            migrationBuilder.UpdateData(
                table: "notifications",
                keyColumn: "Title",
                keyValue: null,
                column: "Title",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "notifications",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 28, 15, 4, 44, 636, DateTimeKind.Local).AddTicks(790), new DateTime(2024, 4, 28, 15, 4, 44, 636, DateTimeKind.Local).AddTicks(776) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "RoleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 4, 28, 15, 4, 44, 636, DateTimeKind.Local).AddTicks(831), new DateTime(2024, 4, 28, 15, 4, 44, 636, DateTimeKind.Local).AddTicks(830) });

            migrationBuilder.AddForeignKey(
                name: "FK_User_notifications_NotificationIdNoti",
                table: "User",
                column: "NotificationIdNoti",
                principalTable: "notifications",
                principalColumn: "IdNoti");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_notifications_NotificationIdNoti",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "NotificationIdNoti",
                table: "User",
                newName: "NotificationId");

            migrationBuilder.RenameIndex(
                name: "IX_User_NotificationIdNoti",
                table: "User",
                newName: "IX_User_NotificationId");

            migrationBuilder.RenameColumn(
                name: "IdNoti",
                table: "notifications",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "notifications",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.AddForeignKey(
                name: "FK_User_notifications_NotificationId",
                table: "User",
                column: "NotificationId",
                principalTable: "notifications",
                principalColumn: "Id");
        }
    }
}
