using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AttendanceManager.Persistance.Migrations
{
    public partial class UpdateUserAccountConfirmed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("53591271-686f-46ad-8edc-16b06a8ef5fe"));

            migrationBuilder.AddColumn<bool>(
                name: "AccountConfirmed",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "AccountConfirmed", "Code", "Email", "EnroleYear", "FullName", "Password", "Role" },
                values: new object[] { new Guid("e6910619-dad9-4462-af8b-3b6db9cf1d1c"), false, "000000", "admin@admin.ro", 0, "Administrator", "system123", 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("e6910619-dad9-4462-af8b-3b6db9cf1d1c"));

            migrationBuilder.DropColumn(
                name: "AccountConfirmed",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Code", "Email", "EnroleYear", "FullName", "Password", "Role" },
                values: new object[] { new Guid("53591271-686f-46ad-8edc-16b06a8ef5fe"), "000000", "admin@admin.ro", 0, "Administrator", "system123", 0 });
        }
    }
}
