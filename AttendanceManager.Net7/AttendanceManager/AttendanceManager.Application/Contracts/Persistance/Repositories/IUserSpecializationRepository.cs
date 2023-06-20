using AttendanceManager.Domain.Entities;
using System.Linq.Expressions;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IUserSpecializationRepository : IGenericRepository<UserSpecialization>
    {
        Task<List<UserSpecialization>> GetUserSpecializationsByExpressionAsync(Expression<Func<UserSpecialization, bool>> expression);
    }
}
