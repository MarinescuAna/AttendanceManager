using AttendanceManager.Domain.Entities;
using System.Linq.Expressions;

namespace AttendanceManager.Application.Contracts.Persistance.Repositories
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        Task<Report?> GetReportByIdAsync(int id);
        Task<List<Report>> GetUserReportsByExpressionAsync(Expression<Func<Report, bool>> expression);
    }
}
