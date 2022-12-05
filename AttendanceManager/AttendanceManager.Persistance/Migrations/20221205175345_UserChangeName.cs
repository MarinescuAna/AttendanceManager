using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AttendanceManager.Persistance.Migrations
{
    public partial class UserChangeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("11b79872-c444-475c-93a4-c9eb443a561c"));

            migrationBuilder.DropColumn(
                name: "EnroleYear",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "EnrollmentYear",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "AccountConfirmed", "Code", "Created", "Email", "EnrollmentYear", "FullName", "Password", "Role", "Updated" },
                values: new object[] { new Guid("fd79b82e-328b-4e0d-9744-77080de2e6d4"), false, "-", new DateTime(2022, 12, 5, 19, 53, 45, 16, DateTimeKind.Local).AddTicks(2599), "admin@admin.ro", 2022, "Administrator", "system123", 0, new DateTime(2022, 12, 5, 19, 53, 45, 18, DateTimeKind.Local).AddTicks(8212) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("fd79b82e-328b-4e0d-9744-77080de2e6d4"));

            migrationBuilder.DropColumn(
                name: "EnrollmentYear",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "EnroleYear",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "AccountConfirmed", "Code", "Created", "Email", "EnroleYear", "FullName", "Password", "Role", "Updated" },
                values: new object[] { new Guid("11b79872-c444-475c-93a4-c9eb443a561c"), false, "-", new DateTime(2022, 12, 4, 17, 9, 38, 212, DateTimeKind.Local).AddTicks(7747), "admin@admin.ro", 2022, "Administrator", "system123", 0, new DateTime(2022, 12, 4, 17, 9, 38, 215, DateTimeKind.Local).AddTicks(2228) });
        }
    }
}
