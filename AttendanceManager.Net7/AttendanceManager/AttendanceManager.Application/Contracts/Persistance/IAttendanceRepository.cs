using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Application.Contracts.Persistance
{
    public interface IAttendanceRepository : IGenericRepository<Attendance>
    {
    }
}
