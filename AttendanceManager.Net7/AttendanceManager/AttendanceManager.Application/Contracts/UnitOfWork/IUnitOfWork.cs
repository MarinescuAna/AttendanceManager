using AttendanceManager.Application.Contracts.Persistance;

namespace AttendanceManager.Application.Contracts.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICourseRepository CourseRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        ISpecializationRepository SpecializationRepository { get; }
        IUserRepository UserRepository { get; }
        IUserSpecializationRepository UserSpecializationRepository { get; }
        Task<bool> CommitAsync();
    }
}
