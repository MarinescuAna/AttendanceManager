using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Persistance.Repositories;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Persistance.Repositories;

namespace AttendanceManager.Persistance.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AttendanceManagerDbContext _dbContext;
        private readonly ILoggingService _loggingSerivce;
        private readonly ICourseRepository? _courseRepository;
        private readonly IDepartmentRepository? _departmentRepository;
        private readonly ISpecializationRepository? _specializationRepository;
        private readonly IUserRepository? _userRepository;
        private readonly IUserSpecializationRepository? _userSpecializationRepository;
        private readonly IMemberRepository? _memberRepository;
        private readonly IDocumentRepository? _documentRepository;
        private readonly ICollectionRepository? _collectionRepository;
        private readonly IAttendanceRepository? _attendanceRepository;
        private readonly IInvolvementCodeRepository? _involvementCodeRepository;
        private readonly IRewardRepository? _rewardRepository;
        private readonly IBadgeRepository? _badgeRepository;
        private readonly INotificationRepository? _notificationRepository;
        public UnitOfWork(AttendanceManagerDbContext dbContext, ILoggingService loggingService)
        {
            _loggingSerivce = loggingService;
            _dbContext = dbContext;
        }
        public INotificationRepository NotificationRepository
        {
            get => _notificationRepository ?? new NotificationRepository(_dbContext, _loggingSerivce);
        }         
        public IBadgeRepository BadgeRepository
        {
            get => _badgeRepository ?? new BadgeRepository(_dbContext, _loggingSerivce);
        }        
        public IRewardRepository RewardRepository
        {
            get => _rewardRepository ?? new RewardRepository(_dbContext, _loggingSerivce);
        }
        public IInvolvementCodeRepository InvolvementCodeRepository
        {
            get => _involvementCodeRepository ?? new InvolvementCodeRepository(_dbContext,_loggingSerivce);
        }
        public IAttendanceRepository AttendanceRepository
        {
            get => _attendanceRepository ?? new AttendanceRepository(_dbContext,_loggingSerivce);
        }
        public ICollectionRepository CollectionRepository
        {
            get => _collectionRepository ?? new CollectionRepository(_dbContext, _loggingSerivce);
        }
        public IMemberRepository MemberRepository
        {
            get => _memberRepository ?? new MemberRepository(_dbContext,_loggingSerivce);
        }
        public IDocumentRepository DocumentRepository
        {
            get => _documentRepository ?? new DocumentRepository(_dbContext, _loggingSerivce);
        }
        public ICourseRepository CourseRepository
        {
            get => _courseRepository ?? new CourseRepository(_dbContext, _loggingSerivce);
        }
        public IDepartmentRepository DepartmentRepository
        {
            get => _departmentRepository ?? new DepartmentRepository(_dbContext, _loggingSerivce);
        }
        public ISpecializationRepository SpecializationRepository
        {
            get => _specializationRepository ?? new SpecializationRepository(_dbContext, _loggingSerivce);
        }
        public IUserRepository UserRepository
        {
            get => _userRepository ?? new UserRepository(_dbContext, _loggingSerivce);
        }
        public IUserSpecializationRepository UserSpecializationRepository
        {
            get => _userSpecializationRepository ?? new UserSpecializationRepository(_dbContext, _loggingSerivce);
        }
        public async Task<bool> CommitAsync(int numberOfRows = 1)
        {
            try
            {
                return (await _dbContext.SaveChangesAsync()) >= numberOfRows;
            }
            catch (Exception ex)
            {
                _loggingSerivce.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod()?.Name);
                return false;
            }
        }
    }
}
