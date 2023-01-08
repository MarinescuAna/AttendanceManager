using AttendanceManager.Application.Modules.Seed;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Persistance.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AttendanceManager.Persistance
{
    public sealed class AttendanceManagerDbContext : DbContext
    {
        private readonly IOptions<AdminSeedSetting> _adminSettings;
        public AttendanceManagerDbContext(DbContextOptions<AttendanceManagerDbContext> option, IOptions<AdminSeedSetting> options) : base(option)
        {
            _adminSettings = options;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserSpecialization> UserSpecializations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed(_adminSettings);
        }
    }
}
