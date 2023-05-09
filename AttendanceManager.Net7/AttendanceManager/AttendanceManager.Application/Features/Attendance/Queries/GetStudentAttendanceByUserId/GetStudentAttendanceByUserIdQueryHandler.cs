using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Core.Shared;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Queries.GetStudentAttendanceByUserId
{
    public sealed class GetStudentAttendanceByUserIdQueryHandler : BaseDocumentFeature, IRequestHandler<GetStudentAttendanceByUserIdQuery, StudentAttendancesDTO[]>
    {
        public GetStudentAttendanceByUserIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public Task<StudentAttendancesDTO[]> Handle(GetStudentAttendanceByUserIdQuery request, CancellationToken cancellationToken)
        {
            var currentUserAsMember = documentMembers!.FirstOrDefault(m => m.UserID == request.UserId)
                ?? throw new BadRequestException("No data available.");

            return Task.FromResult(currentUserAsMember.User!.Attendances!.Where(a => attendanceCollectionsType!.ContainsKey(a.AttendanceCollectionID))
                .Select(a => new StudentAttendancesDTO()
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
