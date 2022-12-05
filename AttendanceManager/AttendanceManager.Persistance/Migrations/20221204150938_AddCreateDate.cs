using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AttendanceManager.Persistance.Migrations
{
    public partial class AddCreateDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("e6910619-dad9-4462-af8b-3b6db9cf1d1c"));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "AccountConfirmed", "Code", "Created", "Email", "EnroleYear", "FullName", "Password", "Role", "Updated" },
                values: new object[] { new Guid("11b79872-c444-475c-93a4-c9eb443a561c"), false, "-", new DateTime(2022, 12, 4, 17, 9, 38, 212, DateTimeKind.Local).AddTicks(7747), "admin@admin.ro", 2022, "Administrator", "system123", 0, new DateTime(2022, 12, 4, 17, 9, 38, 215, DateTimeKind.Local).AddTicks(2228) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("11b79872-c444-475c-93a4-c9eb443a561c"));

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "AccountConfirmed", "Code", "Email", "EnroleYear", "FullName", "Password", "Role" },
                values: new object[] { new Guid("e6910619-dad9-4462-af8b-3b6db9cf1d1c"), false, "000000", "admin@admin.ro", 0, "Administrator", "system123", 0 });
        }
    }
}
