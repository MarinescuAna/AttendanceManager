using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace AttendanceManager.Persistance.Seed
{
    public static class ModelBuilderExtention
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
               new User
               {
                   Email = "admin@admin.ro",
                   Password = "system123",
                   Code="-",
                   Created = DateTime.Now,
                   EnrollmentYear = DateTime.Now.Year,
                   Updated= DateTime.Now,   
                   Role = Domain.Enums.Role.Admin,
                   FullName = "Administrator",
                   UserID = System.Guid.NewGuid(),
                   AccountConfirmed= false,
               }
           );
        }
    }
}
