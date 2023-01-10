using AttendanceManager.Application.Modules.Seed;
using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AttendanceManager.Persistance.Seed
{
    public static class ModelBuilderExtention
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
                   Created = DateTime.Now,
                   EnrollmentYear = DateTime.Now.Year,
                   Updated = DateTime.Now,
                   Role = Domain.Enums.Role.Admin,
                   FullName = adminConfiguration!.Fullname,
                   AccountConfirmed = true,
               }
           );

            modelBuilder.Entity<User>().HasData(
               new User
               {
                   Email = "teacher@test.ro",
                   Password = "system1234",
                   Code = "383gvvv343",
                   Created = DateTime.Now,
                   EnrollmentYear = DateTime.Now.Year,
                   Updated = DateTime.Now,
                   Role = Domain.Enums.Role.Teacher,
                   FullName = "Keven Dietrich",
                   AccountConfirmed = true,
               }
            );

            modelBuilder.Entity<User>().HasData(
               new User
               {
                   Email = "student@test.ro",
                   Password = "system1234",
                   Code = "232dde3w",
                   Created = DateTime.Now,
                   EnrollmentYear = DateTime.Now.Year,
                   Updated = DateTime.Now,
                   Role = Domain.Enums.Role.Student,
                   FullName = "Elliott Cummerata",
                   AccountConfirmed = true,
               }
            );
        }
    }
}
