﻿// <auto-generated />
using System;
using AttendanceManager.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AttendanceManager.Persistance.Migrations
{
    [DbContext(typeof(AttendanceManagerDbContext))]
    partial class AttendanceManagerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<Guid>("DocumentFileID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(254)");

                    b.HasKey("AttendanceID");

                    b.HasIndex("DocumentFileID");

                    b.HasIndex("UserID");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Course", b =>
                {
                    b.Property<Guid>("CourseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<Guid>("SpecializationID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(254)");

                    b.HasKey("CourseID");

                    b.HasIndex("SpecializationID");

                    b.HasIndex("UserID");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Department", b =>
                {
                    b.Property<Guid>("DepartmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("DepartmentID");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Document", b =>
                {
                    b.Property<Guid>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CourseID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
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

                    b.Property<Guid>("SpecializationID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(254)");

                    b.HasKey("DocumentId");

                    b.HasIndex("CourseID");

                    b.HasIndex("SpecializationID");

                    b.HasIndex("UserID");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.DocumentFile", b =>
                {
                    b.Property<Guid>("DocumentFileID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ActivityTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("CourseType")
                        .HasColumnType("int");

                    b.Property<Guid>("DocumentID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("DocumentFileID");

                    b.HasIndex("DocumentID");

                    b.ToTable("DocumentFiles");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Specialization", b =>
                {
                    b.Property<Guid>("SpecializationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DepartmentID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

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
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EnrollmentYear")
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Password")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Email");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Email = "admin@admin.ro",
                            AccountConfirmed = true,
                            Code = "-",
                            Created = new DateTime(2023, 1, 16, 20, 32, 0, 665, DateTimeKind.Local).AddTicks(1579),
                            EnrollmentYear = 2023,
                            FullName = "Administrator",
                            Password = "system123",
                            Role = 0,
                            Updated = new DateTime(2023, 1, 16, 20, 32, 0, 665, DateTimeKind.Local).AddTicks(1631)
                        });
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.UserDocument", b =>
                {
                    b.Property<Guid>("UserDocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DocumentID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(254)");

                    b.HasKey("UserDocumentId");

                    b.HasIndex("DocumentID");

                    b.HasIndex("UserID");

                    b.ToTable("UserDocuments");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.UserSpecialization", b =>
                {
                    b.Property<Guid>("UserSpecializationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SpecializationID")
                        .HasColumnType("uniqueidentifier");

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
                    b.HasOne("AttendanceManager.Domain.Entities.DocumentFile", "DocumentFile")
                        .WithMany("Attendances")
                        .HasForeignKey("DocumentFileID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AttendanceManager.Domain.Entities.User", "User")
                        .WithMany("Attendances")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DocumentFile");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Course", b =>
                {
                    b.HasOne("AttendanceManager.Domain.Entities.Specialization", "Specialization")
                        .WithMany("Courses")
                        .HasForeignKey("SpecializationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AttendanceManager.Domain.Entities.User", "User")
                        .WithMany("Courses")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Specialization");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Document", b =>
                {
                    b.HasOne("AttendanceManager.Domain.Entities.Course", "Course")
                        .WithMany("Documents")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AttendanceManager.Domain.Entities.Specialization", "Specialization")
                        .WithMany("Documents")
                        .HasForeignKey("SpecializationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AttendanceManager.Domain.Entities.User", "User")
                        .WithMany("Documents")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Specialization");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.DocumentFile", b =>
                {
                    b.HasOne("AttendanceManager.Domain.Entities.Document", "Document")
                        .WithMany("DocumentFiles")
                        .HasForeignKey("DocumentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");
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

            modelBuilder.Entity("AttendanceManager.Domain.Entities.UserDocument", b =>
                {
                    b.HasOne("AttendanceManager.Domain.Entities.Document", "Document")
                        .WithMany("UserDocuments")
                        .HasForeignKey("DocumentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AttendanceManager.Domain.Entities.User", "User")
                        .WithMany("UserDocuments")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");

                    b.Navigation("User");
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
                    b.Navigation("DocumentFiles");

                    b.Navigation("UserDocuments");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.DocumentFile", b =>
                {
                    b.Navigation("Attendances");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Specialization", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("Documents");

                    b.Navigation("UserSpecializations");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.User", b =>
                {
                    b.Navigation("Attendances");

                    b.Navigation("Courses");

                    b.Navigation("Documents");

                    b.Navigation("UserDocuments");

                    b.Navigation("UserSpecializations");
                });
#pragma warning restore 612, 618
        }
    }
}
