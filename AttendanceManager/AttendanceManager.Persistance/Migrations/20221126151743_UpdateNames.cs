using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AttendanceManager.Persistance.Migrations
{
    public partial class UpdateNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSpecialisations");

            migrationBuilder.DropTable(
                name: "Specialisations");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("9f682383-5b3d-4cab-a949-594a22e71214"));

            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    SpecializationID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DepartmentID = table.Column<Guid>(nullable: false)
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
                name: "UserSpecializations",
                columns: table => new
                {
                    UserSpecializationID = table.Column<Guid>(nullable: false),
                    UserID = table.Column<Guid>(nullable: false),
                    SpecializationID = table.Column<Guid>(nullable: false)
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
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Code", "Email", "EnroleYear", "FullName", "Password", "Role" },
                values: new object[] { new Guid("91b009c8-a97b-4d0e-a36b-5ef87e6676e1"), null, "admin@admin.ro", null, "Administrator", "system123", 0 });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSpecializations");

            migrationBuilder.DropTable(
                name: "Specializations");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("91b009c8-a97b-4d0e-a36b-5ef87e6676e1"));

            migrationBuilder.CreateTable(
                name: "Specialisations",
                columns: table => new
                {
                    SpecialisationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialisations", x => x.SpecialisationID);
                    table.ForeignKey(
                        name: "FK_Specialisations_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSpecialisations",
                columns: table => new
                {
                    UserSpecialisationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpecialisationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSpecialisations", x => x.UserSpecialisationID);
                    table.ForeignKey(
                        name: "FK_UserSpecialisations_Specialisations_SpecialisationID",
                        column: x => x.SpecialisationID,
                        principalTable: "Specialisations",
                        principalColumn: "SpecialisationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSpecialisations_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Code", "Email", "EnroleYear", "FullName", "Password", "Role" },
                values: new object[] { new Guid("9f682383-5b3d-4cab-a949-594a22e71214"), null, "admin@admin.ro", null, "Administrator", "system123", 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Specialisations_DepartmentID",
                table: "Specialisations",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSpecialisations_SpecialisationID",
                table: "UserSpecialisations",
                column: "SpecialisationID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSpecialisations_UserID",
                table: "UserSpecialisations",
                column: "UserID");
        }
    }
}
