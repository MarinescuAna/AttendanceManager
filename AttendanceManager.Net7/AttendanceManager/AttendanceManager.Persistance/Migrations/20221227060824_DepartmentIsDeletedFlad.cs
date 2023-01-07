using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceManager.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class DepartmentIsDeletedFlad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("c7fd9faf-5c33-49f4-b6dd-ba5e3f3708d0"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Specializations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Departments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "AccountConfirmed", "Code", "Created", "Email", "EnrollmentYear", "FullName", "Password", "Role", "Updated" },
                values: new object[] { new Guid("300ccf99-c355-453d-b6f7-19830d985a64"), false, "-", new DateTime(2022, 12, 27, 8, 8, 24, 505, DateTimeKind.Local).AddTicks(9859), "admin@admin.ro", 2022, "Administrator", "system123", 0, new DateTime(2022, 12, 27, 8, 8, 24, 505, DateTimeKind.Local).AddTicks(9915) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("300ccf99-c355-453d-b6f7-19830d985a64"));

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Specializations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Departments");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "AccountConfirmed", "Code", "Created", "Email", "EnrollmentYear", "FullName", "Password", "Role", "Updated" },
                values: new object[] { new Guid("c7fd9faf-5c33-49f4-b6dd-ba5e3f3708d0"), false, "-", new DateTime(2022, 12, 17, 16, 41, 35, 659, DateTimeKind.Local).AddTicks(1758), "admin@admin.ro", 2022, "Administrator", "system123", 0, new DateTime(2022, 12, 17, 16, 41, 35, 659, DateTimeKind.Local).AddTicks(1824) });
        }
    }
}
