using AttendanceManager.Application.Modules.Seed;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Persistance.ModelBuilderExtention;
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
        public DbSet<Report> Reports { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Involvement> Involvements { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<InvolvementCode> InvolvementCodes { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed(_adminSettings);
            modelBuilder.SetRelationships();
        }
    }
}
