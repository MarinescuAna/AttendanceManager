using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Features.Notification.Commands.CreateNotification;
using AttendanceManager.Application.Features.Reward.Queries.GetAllRewardsByUserIdReportId;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Features.Badge.Commands.InsertCustomBadge
{
    public sealed class InsertCustomBadgeCommand : IRequest<RewardVm>
    {
        public required int MaxNumber { get; init; }
        public required string Title { get; init; }
        public required string BadgeType { get; init; }
        public required string Type { get; init; }
    }

    public sealed class InsertCustomBadgeCommandHandler : IRequestHandler<InsertCustomBadgeCommand, RewardVm>
    {
        private IReportSingleton _currentReport;
        private IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public InsertCustomBadgeCommandHandler(IUnitOfWork unit, IReportSingleton reportSingleton, IMediator mediator)
        {
            _mediator= mediator;
            _currentReport = reportSingleton;
            _unitOfWork = unit;

            if (_currentReport.CurrentReportInfo == null)
            {
                throw new Exceptions.NotImplementedException(ErrorMessages.NoContentReportBaseMessage);
            }
        }
        public async Task<RewardVm> Handle(InsertCustomBadgeCommand request, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse(request.BadgeType, out BadgeType badgeType ) || !Enum.TryParse(request.Type, out ActivityType courseType))
            {
                throw new BadRequestException(ErrorMessages.BadRequest_InvalidParameters_ConversionFaild_Error);
            }

            var newBadge = new Domain.Entities.Badge()
            {
                BadgeType = badgeType,
                CourseType = courseType,
                Description = string.Format(badgeType == BadgeType.CustomAttendanceAchieved ?
                    Constants.CustomAttendanceDescription : Constants.CustomBonusPointsDescription, request.MaxNumber, request.Type.ToString()),
                MaxNumber = request.MaxNumber,
                ImagePath = badgeType == BadgeType.CustomAttendanceAchieved ? Constants.CustomAttendanceImage : Constants.CustomBonusPointsImage,
                Title = request.Title,
                UserRole = Role.Student,
                ReportID = _currentReport.CurrentReportInfo.ReportId
            };

            _unitOfWork.BadgeRepository.AddAsync(newBadge);

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            foreach (var student in _currentReport.Members.Where(m => m.Value == Role.Student))
            {
                await _mediator.Send(new CreateNotificationCommand() { 
                    UserEmail = student.Key,
                    CommitChanges = false,
                    Priority = NotificationPriority.Warning,
                    Message = string.Format(NotificationMessages.CustomBadgeAddedNotification,newBadge.Title,_currentReport.CurrentReportInfo.Title)
                });
            }

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongNotificationsGenericMessage);
            }

            return new RewardVm()
            {
                BadgeId = newBadge.BadgeID,
                BadgeType = newBadge.BadgeType,
                Description = newBadge.Description,
                MaxNumber = newBadge.MaxNumber,
                ImagePath = newBadge.ImagePath,
                Title = newBadge.Title,
                IsCustom = true,
                IsActive = false,
                ActivityType = newBadge.CourseType,
                RewardId = null
            };
        }
    }
}
