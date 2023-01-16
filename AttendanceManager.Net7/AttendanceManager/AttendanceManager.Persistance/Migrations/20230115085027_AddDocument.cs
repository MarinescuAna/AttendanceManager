using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceManager.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnrollmentYear = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(254)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxNoSeminaries = table.Column<int>(type: "int", nullable: false),
                    MaxNoLaboratories = table.Column<int>(type: "int", nullable: false),
                    MaxNoLessons = table.Column<int>(type: "int", nullable: false),
                    CourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpecializationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Documents_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Documents_Specializations_SpecializationID",
                        column: x => x.SpecializationID,
                        principalTable: "Specializations",
                        principalColumn: "SpecializationID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Documents_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UserDocuments",
                columns: table => new
                {
                    UserDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(254)", nullable: false),
                    DocumentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocuments", x => x.UserDocumentId);
                    table.ForeignKey(
                        name: "FK_UserDocuments_Documents_DocumentID",
                        column: x => x.DocumentID,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDocuments_Users_UserID",
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
                values: new object[] { new DateTime(2023, 1, 15, 10, 50, 27, 181, DateTimeKind.Local).AddTicks(6101), new DateTime(2023, 1, 15, 10, 50, 27, 181, DateTimeKind.Local).AddTicks(6163) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "student@test.ro",
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTime(2023, 1, 15, 10, 50, 27, 181, DateTimeKind.Local).AddTicks(6223), new DateTime(2023, 1, 15, 10, 50, 27, 181, DateTimeKind.Local).AddTicks(6227) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "teacher@test.ro",
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTime(2023, 1, 15, 10, 50, 27, 181, DateTimeKind.Local).AddTicks(6204), new DateTime(2023, 1, 15, 10, 50, 27, 181, DateTimeKind.Local).AddTicks(6208) });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CourseID",
                table: "Documents",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_SpecializationID",
                table: "Documents",
                column: "SpecializationID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UserID",
                table: "Documents",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserDocuments_DocumentID",
                table: "UserDocuments",
                column: "DocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_UserDocuments_UserID",
                table: "UserDocuments",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDocuments");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@admin.ro",
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTime(2023, 1, 10, 20, 26, 34, 136, DateTimeKind.Local).AddTicks(1331), new DateTime(2023, 1, 10, 20, 26, 34, 136, DateTimeKind.Local).AddTicks(1383) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "student@test.ro",
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTime(2023, 1, 10, 20, 26, 34, 136, DateTimeKind.Local).AddTicks(1443), new DateTime(2023, 1, 10, 20, 26, 34, 136, DateTimeKind.Local).AddTicks(1446) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "teacher@test.ro",
                columns: new[] { "Created", "Updated" },
                values: new object[] { new DateTime(2023, 1, 10, 20, 26, 34, 136, DateTimeKind.Local).AddTicks(1425), new DateTime(2023, 1, 10, 20, 26, 34, 136, DateTimeKind.Local).AddTicks(1430) });
        }
    }
}
