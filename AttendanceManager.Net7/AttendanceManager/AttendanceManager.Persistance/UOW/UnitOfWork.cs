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
        private readonly IDocumentMemberRepository? _documentMemberRepository;
        private readonly IDocumentRepository? _documentRepository;
        private readonly IAttendanceCollectionRepository? _attendanceCollectionRepository;
        private readonly IAttendanceRepository? _attendanceRepository;
        private readonly IAttendanceCodeRepository? _attendanceCodeRepository;
        private readonly IRewardRepository? _rewardRepository;
        public UnitOfWork(AttendanceManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IRewardRepository RewardRepository
        {
            get => _rewardRepository ?? new RewardRepository(_dbContext);
        }
        public IAttendanceCodeRepository AttendanceCodeRepository
        {
            get => _attendanceCodeRepository ?? new AttendanceCodeRepository(_dbContext);
        }
        public IAttendanceRepository AttendanceRepository
        {
            get => _attendanceRepository ?? new AttendanceRepository(_dbContext);
        }
        public IAttendanceCollectionRepository AttendanceCollectionRepository
        {
            get => _attendanceCollectionRepository ?? new AttendanceCollectionRepository(_dbContext);
        }
        public IDocumentMemberRepository DocumentMemberRepository
        {
            get => _documentMemberRepository ?? new DocumentMemberRepository(_dbContext);
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
        public async Task<bool> CommitAsync(int numberOfRows = 1)
        {
            try
            {
                return (await _dbContext.SaveChangesAsync()) >= numberOfRows;
            }
            catch (Exception ex)
            {
                LoggerSerivce.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
                return false;
            }
        }
    }
}
