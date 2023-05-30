using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.Notification.Commands.ReadNotificationById
{
    public sealed class ReadNotificationByIdCommand : IRequest<bool>
    {
        public required int NotificationId { get; set; }
    }

    public sealed class ReadNotificationByIdCommandHandler : IRequestHandler<ReadNotificationByIdCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReadNotificationByIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(ReadNotificationByIdCommand request, CancellationToken cancellationToken)
        {
            var currentNotification = await _unitOfWork.NotificationRepository.GetAsync(n => n.NotificationID == request.NotificationId)
                ?? throw new NotFoundException("Notification", request.NotificationId);

            currentNotification.IsRead = true;

            _unitOfWork.NotificationRepository.Update(currentNotification);

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrong_NotificationUpdate_Error);
            }

            return true;
        }
    }
}
