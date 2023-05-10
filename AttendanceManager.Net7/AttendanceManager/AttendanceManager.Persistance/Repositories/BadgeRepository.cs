using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Persistance.Repositories
{
    public class BadgeRepository : GenericRepository<Badge>, IBadgeRepository
    {
        public BadgeRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
