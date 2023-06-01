using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Notification.Queries.GetNotificationsByUserId
{
    public sealed class GetNotificationsByUserIdQuery : IRequest<NotificationVm[]>
    {
        public required string UserId { get; set; }
    }

    public sealed class GetNotificationsByUserIdQueryHandler : IRequestHandler<GetNotificationsByUserIdQuery, NotificationVm[]>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetNotificationsByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task<NotificationVm[]> Handle(GetNotificationsByUserIdQuery request, CancellationToken cancellationToken)
            => Task.FromResult(_mapper.Map<NotificationVm[]>(
                _unitOfWork.NotificationRepository.ListAll()
                    .Where(n => n.UserID.Equals(request.UserId))
                    .OrderBy(n => n.IsRead) // Sort by IsRead (ascending)
                    .ThenByDescending(n => n.CreatedOn) // Sort by CreatedOn (descending)
                    .ToArray()));
    }
}
