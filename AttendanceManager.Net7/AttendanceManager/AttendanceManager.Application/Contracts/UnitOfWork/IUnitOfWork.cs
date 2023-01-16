using AttendanceManager.Application.Contracts.Persistance;

namespace AttendanceManager.Application.Contracts.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IAttendanceRepository AttendanceRepository { get; }
        public IDocumentFileRepository DocumentFileRepository { get; }
        ICourseRepository CourseRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        ISpecializationRepository SpecializationRepository { get; }
        IUserRepository UserRepository { get; }
        IUserSpecializationRepository UserSpecializationRepository { get; }
        IUserDocumentRepository UserDocumentRepository { get; }
        IDocumentRepository DocumentRepository { get; }
        Task<bool> CommitAsync();
    }
}
