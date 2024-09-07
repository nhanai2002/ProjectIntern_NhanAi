using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopCore.Migrations
{
    /// <inheritdoc />
    public partial class V3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_notifications_NotificationIdNoti",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_NotificationIdNoti",
                table: "User");

            migrationBuilder.DropColumn(
                name: "NotificationIdNoti",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "IdNoti",
                table: "notifications",
                newName: "NotiId");

            migrationBuilder.CreateTable(
                name: "userNotis",
                columns: table => new
                {
                    IdUserNoti = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NotiId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userNotis", x => x.IdUserNoti);
                    table.ForeignKey(
                        name: "FK_userNotis_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userNotis_notifications_NotiId",
                        column: x => x.NotiId,
                        principalTable: "notifications",
                        principalColumn: "NotiId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateIndex(
                name: "IX_userNotis_NotiId",
                table: "userNotis",
                column: "NotiId");

            migrationBuilder.CreateIndex(
                name: "IX_userNotis_UserId",
                table: "userNotis",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userNotis");

            migrationBuilder.RenameColumn(
                name: "NotiId",
                table: "notifications",
                newName: "IdNoti");

            migrationBuilder.AddColumn<int>(
                name: "NotificationIdNoti",
                table: "User",
                type: "int",
                nullable: true);

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

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserId",
                keyValue: 1,
                column: "NotificationIdNoti",
                value: null);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserId",
                keyValue: 2,
                column: "NotificationIdNoti",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_User_NotificationIdNoti",
                table: "User",
                column: "NotificationIdNoti");

            migrationBuilder.AddForeignKey(
                name: "FK_User_notifications_NotificationIdNoti",
                table: "User",
                column: "NotificationIdNoti",
                principalTable: "notifications",
                principalColumn: "IdNoti");
        }
    }
}
