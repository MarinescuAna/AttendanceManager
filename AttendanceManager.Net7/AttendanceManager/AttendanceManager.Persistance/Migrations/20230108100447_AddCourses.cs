using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceManager.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("300ccf99-c355-453d-b6f7-19830d985a64"));

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SpecializationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseID);
                    table.ForeignKey(
                        name: "FK_Courses_Specializations_SpecializationID",
                        column: x => x.SpecializationID,
                        principalTable: "Specializations",
                        principalColumn: "SpecializationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "AccountConfirmed", "Code", "Created", "Email", "EnrollmentYear", "FullName", "Password", "Role", "Updated" },
                values: new object[] { new Guid("6dba07f3-4364-4c34-989a-e00a0ea6ba87"), false, "-", new DateTime(2023, 1, 8, 12, 4, 46, 857, DateTimeKind.Local).AddTicks(5806), "admin@admin.ro", 2023, "Administrator", "system123", 0, new DateTime(2023, 1, 8, 12, 4, 46, 857, DateTimeKind.Local).AddTicks(5863) });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SpecializationID",
                table: "Courses",
                column: "SpecializationID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("6dba07f3-4364-4c34-989a-e00a0ea6ba87"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "AccountConfirmed", "Code", "Created", "Email", "EnrollmentYear", "FullName", "Password", "Role", "Updated" },
                values: new object[] { new Guid("300ccf99-c355-453d-b6f7-19830d985a64"), false, "-", new DateTime(2022, 12, 27, 8, 8, 24, 505, DateTimeKind.Local).AddTicks(9859), "admin@admin.ro", 2022, "Administrator", "system123", 0, new DateTime(2022, 12, 27, 8, 8, 24, 505, DateTimeKind.Local).AddTicks(9915) });
        }
    }
}
