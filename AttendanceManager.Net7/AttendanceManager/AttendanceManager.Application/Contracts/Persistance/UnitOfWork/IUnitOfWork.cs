﻿using AttendanceManager.Application.Contracts.Persistance.Repositories;

namespace AttendanceManager.Application.Contracts.Persistance.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IInvolvementRepository InvolvementRepository { get; }
        public ICollectionRepository CollectionRepository { get; }
        ICourseRepository CourseRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        ISpecializationRepository SpecializationRepository { get; }
        IUserRepository UserRepository { get; }
        IUserSpecializationRepository UserSpecializationRepository { get; }
        IMemberRepository MemberRepository { get; }
        IReportRepository ReportRepository { get; }
        IInvolvementCodeRepository InvolvementCodeRepository { get; }
        IRewardRepository RewardRepository { get; }
        IBadgeRepository BadgeRepository { get; }
        INotificationRepository NotificationRepository { get; }
        Task<bool> CommitAsync(int numberOfRows = 1);
    }
}
