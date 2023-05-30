using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.Notification.Commands.DeleteNotificationById
{
    public sealed class DeleteNotificationByIdCommand : IRequest<bool>
    {
        public required int NotificationId { get; set; }
    }

    public sealed class DeleteNotificationByIdCommandHandler : IRequestHandler<DeleteNotificationByIdCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteNotificationByIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteNotificationByIdCommand request, CancellationToken cancellationToken)
        {
            var currentNotification = await _unitOfWork.NotificationRepository.GetAsync(n => n.NotificationID == request.NotificationId)
                ?? throw new NotFoundException("Notification", request.NotificationId);

            _unitOfWork.NotificationRepository.Delete(currentNotification);

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrong_NotificationDelete_Error);
            }

            return true;
        }
    }
}
