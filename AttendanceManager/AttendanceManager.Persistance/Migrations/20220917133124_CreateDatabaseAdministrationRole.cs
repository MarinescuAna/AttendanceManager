using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AttendanceManager.Persistance.Migrations
{
    public partial class CreateDatabaseAdministrationRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<Guid>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    EnroleYear = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Specialisations",
                columns: table => new
                {
                    SpecialisationID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DepartmentID = table.Column<Guid>(nullable: false)
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
                    UserSpecialisationID = table.Column<Guid>(nullable: false),
                    UserID = table.Column<Guid>(nullable: false),
                    SpecialisationID = table.Column<Guid>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSpecialisations");

            migrationBuilder.DropTable(
                name: "Specialisations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
