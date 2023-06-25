using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Persistance.Repositories
{

    public sealed class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AttendanceManagerDbContext dbContext, ILoggingService loggingService) : base(dbContext, loggingService)
        {
        }

        public async Task AddRangeAsync(List<User> entities)
        {
            try
            {
                await dbContext.Users.AddRangeAsync(entities);
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
            }
        }
    }
}
