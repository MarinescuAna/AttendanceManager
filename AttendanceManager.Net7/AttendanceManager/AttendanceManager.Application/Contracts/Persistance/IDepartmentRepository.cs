using AttendanceManager.Domain.Entities;

namespace AttendanceManager.Application.Contracts.Persistance
{
    public interface IDepartmentRepository: IGenericRepository<Department>
    {
        Task<bool> SoftOrHardDelete(int id);
    }
}
