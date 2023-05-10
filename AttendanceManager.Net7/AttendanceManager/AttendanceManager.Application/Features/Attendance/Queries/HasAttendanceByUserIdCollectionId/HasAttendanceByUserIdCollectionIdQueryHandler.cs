using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Queries.HasAttendanceByUserIdCollectionId
{
    public sealed class HasAttendanceByUserIdCollectionIdQueryHandler : BaseFeature, IRequestHandler<HasAttendanceByUserIdCollectionIdQuery, bool>
    {
        public HasAttendanceByUserIdCollectionIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<bool> Handle(HasAttendanceByUserIdCollectionIdQuery request, CancellationToken cancellationToken)
            => await unitOfWork.AttendanceRepository.HasAttendanceAsync(a =>
                a.IsPresent &&
                a.UserID.Equals(request.StudentId) &&
                a.AttendanceCollectionID == request.CollectionId);
    }
}
