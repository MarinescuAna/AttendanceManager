using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Persistance.Repositories
{

    public sealed class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AttendanceManagerDbContext dbContext) : base(dbContext)
        {
        }


    }
}
