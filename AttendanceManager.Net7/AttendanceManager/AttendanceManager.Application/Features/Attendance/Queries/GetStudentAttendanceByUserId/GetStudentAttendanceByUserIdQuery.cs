using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Queries.GetStudentAttendanceByUserId
{
    public sealed class GetStudentAttendanceByUserIdQuery : IRequest<StudentAttendancesDto[]>
    {
        public required string UserId { get; init; }
    }

    public sealed class GetStudentAttendanceByUserIdQueryHandler : BaseDocumentFeature, IRequestHandler<GetStudentAttendanceByUserIdQuery, StudentAttendancesDto[]>
    {
        public GetStudentAttendanceByUserIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public Task<StudentAttendancesDto[]> Handle(GetStudentAttendanceByUserIdQuery request, CancellationToken cancellationToken)
        {
            var currentUserAsMember = documentMembers!.FirstOrDefault(m => m.UserID == request.UserId)
                ?? throw new BadRequestException("No data available.");

            return Task.FromResult(currentUserAsMember.User!.Attendances!.Where(a => attendanceCollectionsType!.ContainsKey(a.AttendanceCollectionID))
                .Select(a => new StudentAttendancesDto()
                {
                    UserId = a.UserID,
                    AttendanceId = a.AttendanceID,
                    BonusPoints = a.BonusPoints,
                    IsPresent = a.IsPresent,
                    UpdatedOn = a.UpdatedOn.ToString(Constants.DateFormat),
                    CourseType = attendanceCollectionsType![a.AttendanceCollectionID].ToString()
                }).ToArray());
        }
    }
}
