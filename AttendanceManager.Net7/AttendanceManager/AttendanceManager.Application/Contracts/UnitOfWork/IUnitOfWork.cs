using AttendanceManager.Application.Contracts.Persistance;

namespace AttendanceManager.Application.Contracts.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IAttendanceRepository AttendanceRepository { get; }
        public IAttendanceCollectionRepository AttendanceCollectionRepository { get; }
        ICourseRepository CourseRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        ISpecializationRepository SpecializationRepository { get; }
        IUserRepository UserRepository { get; }
        IUserSpecializationRepository UserSpecializationRepository { get; }
        IDocumentMemberRepository DocumentMemberRepository { get; }
        IDocumentRepository DocumentRepository { get; }
        IAttendanceCodeRepository AttendanceCodeRepository { get; }
        Task<bool> CommitAsync(int numberOfRows = 1);
    }
}
