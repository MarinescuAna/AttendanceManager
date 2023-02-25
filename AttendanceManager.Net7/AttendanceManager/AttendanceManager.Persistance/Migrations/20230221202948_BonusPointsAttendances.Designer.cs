﻿// <auto-generated />
using System;
using AttendanceManager.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AttendanceManager.Persistance.Migrations
{
    [DbContext(typeof(AttendanceManagerDbContext))]
    [Migration("20230221202948_BonusPointsAttendances")]
    partial class BonusPointsAttendances
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Attendance", b =>
                {
                    b.Property<int>("AttendanceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AttendanceID"));

                    b.Property<int>("AttendanceCollectionID")
                        .HasColumnType("int");

                    b.Property<int>("BonusPoints")
                        .HasColumnType("int");

                    b.Property<bool>("IsPresent")
                        .HasColumnType("bit");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(254)");

                    b.HasKey("AttendanceID");

                    b.HasIndex("AttendanceCollectionID");

                    b.HasIndex("UserID");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.AttendanceCollection", b =>
                {
                    b.Property<int>("AttendanceCollectionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AttendanceCollectionID"));

                    b.Property<int>("CourseType")
                        .HasColumnType("int");

                    b.Property<int>("DocumentID")
                        .HasColumnType("int");

                    b.Property<DateTime>("HeldOn")
                        .HasColumnType("datetime2");

                    b.HasKey("AttendanceCollectionID");

                    b.HasIndex("DocumentID");

                    b.ToTable("AttendanceCollections");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Course", b =>
                {
                    b.Property<int>("CourseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseID"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserSpecializationID")
                        .HasColumnType("int");

                    b.HasKey("CourseID");

                    b.HasIndex("UserSpecializationID");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Department", b =>
                {
                    b.Property<int>("DepartmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentID"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("DepartmentID");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Document", b =>
                {
                    b.Property<int>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentId"));

                    b.Property<int>("CourseID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("EnrollmentYear")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("MaxNoLaboratories")
                        .HasColumnType("int");

                    b.Property<int>("MaxNoLessons")
                        .HasColumnType("int");

                    b.Property<int>("MaxNoSeminaries")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("DocumentId");

                    b.HasIndex("CourseID");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.DocumentMember", b =>
                {
                    b.Property<int>("DocumentMemberID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentMemberID"));

                    b.Property<int>("DocumentID")
                        .HasColumnType("int");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(254)");

                    b.HasKey("DocumentMemberID");

                    b.HasIndex("DocumentID");

                    b.HasIndex("UserID");

                    b.ToTable("DocumentMember");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Specialization", b =>
                {
                    b.Property<int>("SpecializationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SpecializationID"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("DepartmentID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("SpecializationID");

                    b.HasIndex("DepartmentID");

                    b.ToTable("Specializations");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.User", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

                    b.Property<bool>("AccountConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("EnrollmentYear")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ExpRefreshToken")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Email");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Email = "admin@admin.ro",
                            AccountConfirmed = true,
                            Code = "-",
                            CreatedOn = new DateTime(2023, 2, 21, 22, 29, 48, 180, DateTimeKind.Local).AddTicks(7934),
                            EnrollmentYear = 2023,
                            FullName = "Administrator",
                            IsDeleted = false,
                            Password = "system123",
                            Role = 0,
                            UpdatedOn = new DateTime(2023, 2, 21, 22, 29, 48, 180, DateTimeKind.Local).AddTicks(8011)
                        });
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.UserSpecialization", b =>
                {
                    b.Property<int>("UserSpecializationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserSpecializationID"));

                    b.Property<int>("SpecializationID")
                        .HasColumnType("int");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(254)");

                    b.HasKey("UserSpecializationID");

                    b.HasIndex("SpecializationID");

                    b.HasIndex("UserID");

                    b.ToTable("UserSpecializations");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Attendance", b =>
                {
                    b.HasOne("AttendanceManager.Domain.Entities.AttendanceCollection", "AttendanceCollection")
                        .WithMany("Attendances")
                        .HasForeignKey("AttendanceCollectionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AttendanceManager.Domain.Entities.User", "User")
                        .WithMany("Attendances")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AttendanceCollection");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.AttendanceCollection", b =>
                {
                    b.HasOne("AttendanceManager.Domain.Entities.Document", "Document")
                        .WithMany("AttendanceCollections")
                        .HasForeignKey("DocumentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Course", b =>
                {
                    b.HasOne("AttendanceManager.Domain.Entities.UserSpecialization", "UserSpecialization")
                        .WithMany("Courses")
                        .HasForeignKey("UserSpecializationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserSpecialization");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Document", b =>
                {
                    b.HasOne("AttendanceManager.Domain.Entities.Course", "Course")
                        .WithMany("Documents")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.DocumentMember", b =>
                {
                    b.HasOne("AttendanceManager.Domain.Entities.Document", "Document")
                        .WithMany("DocumentMembers")
                        .HasForeignKey("DocumentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AttendanceManager.Domain.Entities.User", "User")
                        .WithMany("DocumentMembers")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Specialization", b =>
                {
                    b.HasOne("AttendanceManager.Domain.Entities.Department", "Department")
                        .WithMany("Specializations")
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.UserSpecialization", b =>
                {
                    b.HasOne("AttendanceManager.Domain.Entities.Specialization", "Specialization")
                        .WithMany("UserSpecializations")
                        .HasForeignKey("SpecializationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AttendanceManager.Domain.Entities.User", "User")
                        .WithMany("UserSpecializations")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Specialization");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.AttendanceCollection", b =>
                {
                    b.Navigation("Attendances");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Course", b =>
                {
                    b.Navigation("Documents");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Department", b =>
                {
                    b.Navigation("Specializations");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Document", b =>
                {
                    b.Navigation("AttendanceCollections");

                    b.Navigation("DocumentMembers");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Specialization", b =>
                {
                    b.Navigation("UserSpecializations");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.User", b =>
                {
                    b.Navigation("Attendances");

                    b.Navigation("DocumentMembers");

                    b.Navigation("UserSpecializations");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.UserSpecialization", b =>
                {
                    b.Navigation("Courses");
                });
#pragma warning restore 612, 618
        }
    }
}