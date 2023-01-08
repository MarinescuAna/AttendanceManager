using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceManager.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCodeLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("6dba07f3-4364-4c34-989a-e00a0ea6ba87"));

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "AccountConfirmed", "Code", "Created", "Email", "EnrollmentYear", "FullName", "Password", "Role", "Updated" },
                values: new object[] { new Guid("d217f65c-89af-4637-a1c8-df5cd0e991a6"), true, "-", new DateTime(2023, 1, 8, 13, 28, 29, 875, DateTimeKind.Local).AddTicks(6646), "admin@admin.ro", 2023, "Administrator", "system123", 0, new DateTime(2023, 1, 8, 13, 28, 29, 875, DateTimeKind.Local).AddTicks(6707) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("d217f65c-89af-4637-a1c8-df5cd0e991a6"));

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Users",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "AccountConfirmed", "Code", "Created", "Email", "EnrollmentYear", "FullName", "Password", "Role", "Updated" },
                values: new object[] { new Guid("6dba07f3-4364-4c34-989a-e00a0ea6ba87"), false, "-", new DateTime(2023, 1, 8, 12, 4, 46, 857, DateTimeKind.Local).AddTicks(5806), "admin@admin.ro", 2023, "Administrator", "system123", 0, new DateTime(2023, 1, 8, 12, 4, 46, 857, DateTimeKind.Local).AddTicks(5863) });
        }
    }
}
