using AttendanceManager.Domain.Entities;
using System.Linq.Expressions;

namespace AttendanceManager.Application.Contracts.Persistance
{
    public interface IUserSpecializationRepository: IGenericRepository<UserSpecialization>
    {
        Task<List<UserSpecialization>> GetUserSpecializationsByExpression(Expression<Func<UserSpecialization, bool>> expression);
    }
}
