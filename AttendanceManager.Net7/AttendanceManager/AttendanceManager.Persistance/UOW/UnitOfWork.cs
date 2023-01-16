using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Infrastructure.Shared.Logger;
using AttendanceManager.Persistance.Repositories;

namespace AttendanceManager.Persistance.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AttendanceManagerDbContext _dbContext;
        private readonly ICourseRepository? _courseRepository;
        private readonly IDepartmentRepository? _departmentRepository;
        private readonly ISpecializationRepository? _specializationRepository;
        private readonly IUserRepository? _userRepository;
        private readonly IUserSpecializationRepository? _userSpecializationRepository;
        private readonly IUserDocumentRepository? _userDocumentRepository;
        private readonly IDocumentRepository? _documentRepository;
        private readonly IDocumentFileRepository? _documentFileRepository;
        private readonly IAttendanceRepository? _attendanceRepository;
        public UnitOfWork(AttendanceManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IAttendanceRepository AttendanceRepository
        {
            get => _attendanceRepository ?? new AttendanceRepository(_dbContext);
        }
        public IDocumentFileRepository DocumentFileRepository
        {
            get => _documentFileRepository ?? new DocumentFileRepository(_dbContext);
        }
        public IUserDocumentRepository UserDocumentRepository
        {
            get => _userDocumentRepository ?? new UserDocumentRepository(_dbContext);
        }
        public IDocumentRepository DocumentRepository
        {
            get => _documentRepository ?? new DocumentRepository(_dbContext);
        }
        public ICourseRepository CourseRepository
        {
            get => _courseRepository ?? new CourseRepository(_dbContext);
        }
        public IDepartmentRepository DepartmentRepository
        {
            get => _departmentRepository ?? new DepartmentRepository(_dbContext);
        }
        public ISpecializationRepository SpecializationRepository
        {
            get => _specializationRepository ?? new SpecializationRepository(_dbContext);
        }
        public IUserRepository UserRepository
        {
            get => _userRepository ?? new UserRepository(_dbContext);
        }
        public IUserSpecializationRepository UserSpecializationRepository
        {
            get => _userSpecializationRepository ?? new UserSpecializationRepository(_dbContext);
        }
        public async Task<bool> CommitAsync()
        {
            try
            {
                return (await _dbContext.SaveChangesAsync()) > 0;
            }
            catch (Exception ex)
            {
                LoggerSerivce.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
                return false;
            }
        }
    }
}
