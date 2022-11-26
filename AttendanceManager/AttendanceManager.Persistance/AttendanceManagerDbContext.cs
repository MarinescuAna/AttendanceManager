using AttendanceManager.Domain.Entities;
using AttendanceManager.Persistance.Seed;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManager.Persistance
{
    public class AttendanceManagerDbContext:DbContext
    {
        public AttendanceManagerDbContext(DbContextOptions<AttendanceManagerDbContext> option): base(option)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserSpecialization> UserSpecializations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Specialization> Specializations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }
    }
}
