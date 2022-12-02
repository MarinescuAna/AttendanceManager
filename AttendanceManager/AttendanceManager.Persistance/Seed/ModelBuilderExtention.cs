using AttendanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
                   Role = Domain.Enums.Role.Admin,
                   FullName = "Administrator",
                   UserID = System.Guid.NewGuid(),
                   AccountConfirmed= false,
               }
           );
        }
    }
}
