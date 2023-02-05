using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceManager.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshToken2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@admin.ro",
                columns: new[] { "CreatedOn", "UpdatedOn" },
                values: new object[] { new DateTime(2023, 2, 5, 15, 0, 35, 221, DateTimeKind.Local).AddTicks(3049), new DateTime(2023, 2, 5, 15, 0, 35, 221, DateTimeKind.Local).AddTicks(3102) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@admin.ro",
                columns: new[] { "CreatedOn", "UpdatedOn" },
                values: new object[] { new DateTime(2023, 2, 5, 14, 31, 40, 786, DateTimeKind.Local).AddTicks(9706), new DateTime(2023, 2, 5, 14, 31, 40, 786, DateTimeKind.Local).AddTicks(9763) });
        }
    }
}
