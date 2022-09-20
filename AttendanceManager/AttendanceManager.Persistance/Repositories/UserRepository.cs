using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Persistance.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }

    }
}
