using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceManager.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpRefreshToken",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "nvarchar(64)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@admin.ro",
                columns: new[] { "CreatedOn", "ExpRefreshToken", "RefreshToken", "UpdatedOn" },
                values: new object[] { new DateTime(2023, 2, 5, 14, 31, 40, 786, DateTimeKind.Local).AddTicks(9706), null, null, new DateTime(2023, 2, 5, 14, 31, 40, 786, DateTimeKind.Local).AddTicks(9763) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpRefreshToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@admin.ro",
                columns: new[] { "CreatedOn", "UpdatedOn" },
                values: new object[] { new DateTime(2023, 1, 28, 18, 18, 41, 540, DateTimeKind.Local).AddTicks(3427), new DateTime(2023, 1, 28, 18, 18, 41, 540, DateTimeKind.Local).AddTicks(3497) });
        }
    }
}
