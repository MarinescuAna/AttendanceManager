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
                    b.Property<Guid>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("AccountConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Code")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

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

                    b.HasKey("UserID");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserID = new Guid("300ccf99-c355-453d-b6f7-19830d985a64"),
                            AccountConfirmed = false,
                            Code = "-",
                            Created = new DateTime(2022, 12, 27, 8, 8, 24, 505, DateTimeKind.Local).AddTicks(9859),
                            Email = "admin@admin.ro",
                            EnrollmentYear = 2022,
                            FullName = "Administrator",
                            Password = "system123",
                            Role = 0,
                            Updated = new DateTime(2022, 12, 27, 8, 8, 24, 505, DateTimeKind.Local).AddTicks(9915)
                        });
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.UserSpecialization", b =>
                {
                    b.Property<Guid>("UserSpecializationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SpecializationID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserSpecializationID");

                    b.HasIndex("SpecializationID");

                    b.HasIndex("UserID");

                    b.ToTable("UserSpecializations");
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

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Department", b =>
                {
                    b.Navigation("Specializations");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.Specialization", b =>
                {
                    b.Navigation("UserSpecializations");
                });

            modelBuilder.Entity("AttendanceManager.Domain.Entities.User", b =>
                {
                    b.Navigation("UserSpecializations");
                });
#pragma warning restore 612, 618
        }
    }
}
