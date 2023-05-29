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
                   EnrollmentYear = DateTime.Now.Year,
                   UpdatedOn = DateTime.Now,
                   Role = Domain.Enums.Role.Admin,
                   FullName = adminConfiguration!.Fullname,
                   AccountConfirmed = true,
               }
           );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   Title = "Met your teacher",
                   ImagePath = "first_attendance.jpg",
                   Description = "Achieve this badge by getting the first attendance to any of the available activities.",
                   BadgeID = Domain.Enums.BadgeID.FirstAttendance,
                   UserRole = Domain.Enums.Role.Student
               }
           );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   Title = "Half-master of theory",
                   ImagePath = "lessons_50.jpg",
                   Description = "Achieve this badge by getting half of the attendance to lecture.",
                   BadgeID = Domain.Enums.BadgeID.LecturesAttendances50,
                   UserRole = Domain.Enums.Role.Student
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   Title = "Half-master of practice",
                   ImagePath = "laboratory_50.jpg",
                   Description = "Achieve this badge by getting half of the attendance to laboratory.",
                   BadgeID = Domain.Enums.BadgeID.LaboratoriesAttendances50,
                   UserRole = Domain.Enums.Role.Student
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   Title = "Half-master of seminaries",
                   ImagePath = "seminary_50.jpg",
                   Description = "Achieve this badge by getting half of the attendance to seminary.",
                   BadgeID = Domain.Enums.BadgeID.SeminariesAttendances50,
                   UserRole = Domain.Enums.Role.Student
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID = Domain.Enums.BadgeID.LastAttendance,
                   Title = "First goodbye!",
                   ImagePath = "last_attendance.jpg",
                   Description = "Achieve this badge by getting the last attendance to any of the available activities.",
                   UserRole = Domain.Enums.Role.Student
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   Title = "Master of theory",
                   ImagePath = "complete_lecture.jpg",
                   Description = "Achieve this badge by getting all the attendance to lecture.",
                   BadgeID = Domain.Enums.BadgeID.LecturesAttendancesComplete,
                   UserRole = Domain.Enums.Role.Student
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   Title = "Master of practice",
                   ImagePath = "complete_laboratory.jpg",
                   Description = "Achieve this badge by getting all the attendance to laboratory.",
                   BadgeID = Domain.Enums.BadgeID.LaboratoriesAttendancesComplete,
                   UserRole = Domain.Enums.Role.Student
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   Title = "Master of seminaries",
                   ImagePath = "complete_seminary.jpg",
                   Description = "Achieve this badge by getting all the attendance to seminary.",
                   BadgeID = Domain.Enums.BadgeID.SeminariesAttendancesComplete,
                   UserRole = Domain.Enums.Role.Student
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   Title = "First bonus",
                   ImagePath = "first_point.jpg",
                   Description = "Achieve this badge by getting the first bonus point to any of the available activities.",
                   BadgeID = Domain.Enums.BadgeID.FirstBonus,
                   UserRole = Domain.Enums.Role.Student
               }
            );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   Title = "Smart owl",
                   ImagePath = "smart_owl.jpg",
                   Description = "Achieve this badge by having the greater point of any of the available activities.",
                   BadgeID = Domain.Enums.BadgeID.SmartOwl,
                   UserRole = Domain.Enums.Role.Student
               }
            );
        }
    }
}
