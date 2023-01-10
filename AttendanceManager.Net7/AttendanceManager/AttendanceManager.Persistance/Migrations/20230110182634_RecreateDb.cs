using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AttendanceManager.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class RecreateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    EnrollmentYear = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountConfirmed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    SpecializationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DepartmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializations", x => x.SpecializationID);
                    table.ForeignKey(
                        name: "FK_Specializations_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SpecializationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(254)", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_Courses_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSpecializations",
                columns: table => new
                {
                    UserSpecializationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(254)", nullable: false),
                    SpecializationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSpecializations", x => x.UserSpecializationID);
                    table.ForeignKey(
                        name: "FK_UserSpecializations_Specializations_SpecializationID",
                        column: x => x.SpecializationID,
                        principalTable: "Specializations",
                        principalColumn: "SpecializationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSpecializations_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "AccountConfirmed", "Code", "Created", "EnrollmentYear", "FullName", "Password", "Role", "Updated" },
                values: new object[,]
                {
                    { "admin@admin.ro", true, "-", new DateTime(2023, 1, 10, 20, 26, 34, 136, DateTimeKind.Local).AddTicks(1331), 2023, "Administrator", "system123", 0, new DateTime(2023, 1, 10, 20, 26, 34, 136, DateTimeKind.Local).AddTicks(1383) },
                    { "student@test.ro", true, "232dde3w", new DateTime(2023, 1, 10, 20, 26, 34, 136, DateTimeKind.Local).AddTicks(1443), 2023, "Elliott Cummerata", "system1234", 1, new DateTime(2023, 1, 10, 20, 26, 34, 136, DateTimeKind.Local).AddTicks(1446) },
                    { "teacher@test.ro", true, "383gvvv343", new DateTime(2023, 1, 10, 20, 26, 34, 136, DateTimeKind.Local).AddTicks(1425), 2023, "Keven Dietrich", "system1234", 2, new DateTime(2023, 1, 10, 20, 26, 34, 136, DateTimeKind.Local).AddTicks(1430) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SpecializationID",
                table: "Courses",
                column: "SpecializationID");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_UserID",
                table: "Courses",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_DepartmentID",
                table: "Specializations",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSpecializations_SpecializationID",
                table: "UserSpecializations",
                column: "SpecializationID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSpecializations_UserID",
                table: "UserSpecializations",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "UserSpecializations");

            migrationBuilder.DropTable(
                name: "Specializations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
