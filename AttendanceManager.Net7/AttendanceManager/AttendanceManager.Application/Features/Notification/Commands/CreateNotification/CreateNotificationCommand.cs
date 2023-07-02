using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Features.Notification.Commands.CreateNotification
{
    public sealed class CreateNotificationCommand : IRequest<bool>
    {
        public required string UserEmail { get; init; }
        public required string Message { get; init; }
        public required NotificationPriority Priority { get; init; }
        public bool CommitChanges { get; init; } = true;
    }

    public sealed class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateNotificationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.NotificationRepository.AddAsync(new()
            {
                Priority= request.Priority,
                UserID = request.UserEmail,
                CreatedOn = DateTime.Now,
                IsRead = false,
                Message = request.Message,
            });

            if (request.CommitChanges && !await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrong_NotificationInsert_Error);
            }

            return true;
        }
    }
}
