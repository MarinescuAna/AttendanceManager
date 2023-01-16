using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AttendanceManager.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentFileAndAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "student@test.ro");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "teacher@test.ro");

            migrationBuilder.CreateTable(
                name: "DocumentFiles",
                columns: table => new
                {
                    DocumentFileID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentFiles", x => x.DocumentFileID);
                    table.ForeignKey(
                        name: "FK_DocumentFiles_Documents_DocumentID",
                        column: x => x.DocumentID,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttendanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentFileID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(254)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.AttendanceID);
                    table.ForeignKey(
                        name: "FK_Attendances_DocumentFiles_DocumentFileID",
                        column: x => x.DocumentFileID,
                        principalTable: "DocumentFiles",
                        principalColumn: "DocumentFileID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@admin.ro",
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTime(2023, 1, 16, 20, 32, 0, 665, DateTimeKind.Local).AddTicks(1579), new DateTime(2023, 1, 16, 20, 32, 0, 665, DateTimeKind.Local).AddTicks(1631) });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_DocumentFileID",
                table: "Attendances",
                column: "DocumentFileID");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_UserID",
                table: "Attendances",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentFiles_DocumentID",
                table: "DocumentFiles",
                column: "DocumentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "DocumentFiles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@admin.ro",
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTime(2023, 1, 15, 10, 50, 27, 181, DateTimeKind.Local).AddTicks(6101), new DateTime(2023, 1, 15, 10, 50, 27, 181, DateTimeKind.Local).AddTicks(6163) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "AccountConfirmed", "Code", "Created", "EnrollmentYear", "FullName", "Password", "Role", "Updated" },
                values: new object[,]
                {
                    { "student@test.ro", true, "232dde3w", new DateTime(2023, 1, 15, 10, 50, 27, 181, DateTimeKind.Local).AddTicks(6223), 2023, "Elliott Cummerata", "system1234", 1, new DateTime(2023, 1, 15, 10, 50, 27, 181, DateTimeKind.Local).AddTicks(6227) },
                    { "teacher@test.ro", true, "383gvvv343", new DateTime(2023, 1, 15, 10, 50, 27, 181, DateTimeKind.Local).AddTicks(6204), 2023, "Keven Dietrich", "system1234", 2, new DateTime(2023, 1, 15, 10, 50, 27, 181, DateTimeKind.Local).AddTicks(6208) }
                });
        }
    }
}
