using AttendanceManager.Application.Modules.Seed;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AttendanceManager.Persistance.ModelBuilderExtention
{
    public static class SeedModelBuilder
    {
        public static void Seed(this ModelBuilder modelBuilder, IOptions<AdminSeedSetting> options)
        {
            var adminConfiguration = options.Value;

            modelBuilder.Entity<User>().HasData(
               new User
               {
                   Email = adminConfiguration!.Email,
                   Password = adminConfiguration!.Password,
                   Code = adminConfiguration!.Code,
                   CreatedOn = DateTime.Now,
                   Year = DateTime.Now.Year,
                   UpdatedOn = DateTime.Now,
                   Role = Domain.Enums.Role.Admin,
                   FullName = adminConfiguration!.Fullname,
                   AccountConfirmed = true,
               }
           );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID=1,
                   Title = "Met your teacher",
                   ImagePath = "first_attendance.jpg",
                   Description = "Achieve this badge by getting the first attendance to any of the available activities.",
                   BadgeType = Domain.Enums.BadgeType.FirstAttendance,
                   UserRole = Domain.Enums.Role.Student
               }
           );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID = 2,
                   Title = "Half-master of theory",
                   ImagePath = "lessons_50.jpg",
                   Description = "Achieve this badge by getting half of the attendance to lecture.",
                   BadgeType = Domain.Enums.BadgeType.LecturesAttendances50,
                   UserRole = Domain.Enums.Role.Student
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID = 3,
                   Title = "Half-master of practice",
                   ImagePath = "laboratory_50.jpg",
                   Description = "Achieve this badge by getting half of the attendance to laboratory.",
                   BadgeType = Domain.Enums.BadgeType.LaboratoriesAttendances50,
                   UserRole = Domain.Enums.Role.Student
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID= 4,
                   Title = "Half-master of seminaries",
                   ImagePath = "seminary_50.jpg",
                   Description = "Achieve this badge by getting half of the attendance to seminary.",
                   BadgeType = Domain.Enums.BadgeType.SeminariesAttendances50,
                   UserRole = Domain.Enums.Role.Student
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID= 5,
                   BadgeType = Domain.Enums.BadgeType.LastAttendance,
                   Title = "First goodbye!",
                   ImagePath = "last_attendance.jpg",
                   Description = "Achieve this badge by getting the last attendance to any of the available activities.",
                   UserRole = Domain.Enums.Role.Student
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID= 6,
                   Title = "Master of theory",
                   ImagePath = "complete_lecture.jpg",
                   Description = "Achieve this badge by getting all the attendance to lecture.",
                   BadgeType = Domain.Enums.BadgeType.LecturesAttendancesComplete,
                   UserRole = Domain.Enums.Role.Student
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID= 7,
                   Title = "Master of practice",
                   ImagePath = "complete_laboratory.jpg",
                   Description = "Achieve this badge by getting all the attendance to laboratory.",
                   BadgeType = Domain.Enums.BadgeType.LaboratoriesAttendancesComplete,
                   UserRole = Domain.Enums.Role.Student
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID= 8,
                   Title = "Master of seminaries",
                   ImagePath = "complete_seminary.jpg",
                   Description = "Achieve this badge by getting all the attendance to seminary.",
                   BadgeType = Domain.Enums.BadgeType.SeminariesAttendancesComplete,
                   UserRole = Domain.Enums.Role.Student
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID= 9,
                   Title = "First bonus",
                   ImagePath = "first_point.jpg",
                   Description = "Achieve this badge by getting the first bonus point to any of the available activities.",
                   BadgeType = Domain.Enums.BadgeType.FirstBonus,
                   UserRole = Domain.Enums.Role.Student
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID= 10,
                   Title = "Smart owl",
                   ImagePath = "smart_owl.jpg",
                   Description = "Achieve this badge by having the greater point of any of the available activities.",
                   BadgeType = Domain.Enums.BadgeType.SmartOwl,
                   UserRole = Domain.Enums.Role.Student
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID= 11,
                   Title = "First code generated",
                   ImagePath = "first_code_generated.jpg",
                   Description = "Achieve this badge by generating the first involvement code.",
                   BadgeType = Domain.Enums.BadgeType.FirstCodeGenerated,
                   UserRole = Domain.Enums.Role.Teacher
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID= 12,
                   Title = "First code used",
                   ImagePath = "first_code_used.jpg",
                   Description = "Achieve this badge when the generated code is used for the first time.",
                   BadgeType = Domain.Enums.BadgeType.FirstCodeUsed,
                   UserRole = Domain.Enums.Role.Teacher
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID= 13,
                   Title = "Full class",
                   ImagePath = "first_class_full.jpg",
                   Description = "Achieve this badge when you have all the students at any activity.",
                   BadgeType = Domain.Enums.BadgeType.FullClass,
                   UserRole = Domain.Enums.Role.Teacher
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID = 14,
                   Title = "Good teacher",
                   ImagePath = "good_teacher.jpg",
                   Description = "Achieve this badge when half of the students achieved attendance at any activtiy.",
                   BadgeType = Domain.Enums.BadgeType.GoodTeacher,
                   UserRole = Domain.Enums.Role.Teacher
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID = 15,
                   Title = "Best teacher",
                   ImagePath = "best_teacher.jpg",
                   Description = "Achieve this badge when half of the students achieved more than half of the attendance at any activity.",
                   BadgeType = Domain.Enums.BadgeType.BestTeacher,
                   UserRole = Domain.Enums.Role.Teacher
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID = 16,
                   Title = "Good bye laboratory",
                   ImagePath = "last_laboratory.jpg",
                   Description = "Achieve this badge at the last laboratory if more than half of the students are present.",
                   BadgeType = Domain.Enums.BadgeType.SayByeLaboratory,
                   UserRole = Domain.Enums.Role.Teacher
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID = 17,
                   Title = "Good bye lecture",
                   ImagePath = "last_lecture.jpg",
                   Description = "Achieve this badge at the last lecture if more than half of the students are present.",
                   BadgeType = Domain.Enums.BadgeType.SayByeLecture,
                   UserRole = Domain.Enums.Role.Teacher
               }
            );


            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID = 18,
                   Title = "Good bye seminary",
                   ImagePath = "last_seminary.jpg",
                   Description = "Achieve this badge at the last seminary if more than half of the students are present.",
                   BadgeType = Domain.Enums.BadgeType.SayByeSeminary,
                   UserRole = Domain.Enums.Role.Teacher
               }
            );

        }
    }
}
