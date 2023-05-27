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
                   Title = "First attendance",
                   ImagePath = "first_attendance.jpg",
                   Description = "Achieve this badge by getting the first attendance to any of the available activities.",
                   BadgeID = Domain.Enums.BadgeID.FirstAttendance,
                   UserRole = Domain.Enums.Role.Student
               }
           );

            modelBuilder.Entity<Badge>().HasData(
               new Badge
               {
                   BadgeID = Domain.Enums.BadgeID.LastAttendance,
                   Title = "Last attendance",
                   ImagePath = "last_attendance.jpg",
                   Description = "Achieve this badge by getting the last attendance to any of the available activities.",
                   UserRole = Domain.Enums.Role.Student
               }
            );
        }
    }
}
