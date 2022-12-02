using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AttendanceManager.Persistance.Migrations
{
    public partial class UpdateUserEnrollDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("91b009c8-a97b-4d0e-a36b-5ef87e6676e1"));

            migrationBuilder.AlterColumn<int>(
                name: "EnroleYear",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Code", "Email", "EnroleYear", "FullName", "Password", "Role" },
                values: new object[] { new Guid("53591271-686f-46ad-8edc-16b06a8ef5fe"), "000000", "admin@admin.ro", 0, "Administrator", "system123", 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("53591271-686f-46ad-8edc-16b06a8ef5fe"));

            migrationBuilder.AlterColumn<string>(
                name: "EnroleYear",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Code", "Email", "EnroleYear", "FullName", "Password", "Role" },
                values: new object[] { new Guid("91b009c8-a97b-4d0e-a36b-5ef87e6676e1"), null, "admin@admin.ro", null, "Administrator", "system123", 0 });
        }
    }
}
