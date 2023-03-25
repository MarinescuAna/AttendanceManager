using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Core.Shared;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Queries.GetStudentAttendanceByDocIdAndUserId
{
    public sealed class GetStudentAttendanceByDocIdAndUserIdQueryHandler : BaseFeature, IRequestHandler<GetStudentAttendanceByDocIdAndUserIdQuery, StudentAttendancesDTO[]>
    {
        public GetStudentAttendanceByDocIdAndUserIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<StudentAttendancesDTO[]> Handle(GetStudentAttendanceByDocIdAndUserIdQuery request, CancellationToken cancellationToken)
        {
            var documentMember = await unitOfWork.DocumentMemberRepository.GetMemberByDocumentIdAndUserIdAsync(request.DocumentId, request.UserId)
                ?? throw new BadRequestException("No data available.");
            var attendanceCollections = documentMember!.Document!.AttendanceCollections!.ToDictionary(ac => ac.AttendanceCollectionID, ac => ac.CourseType);

            return documentMember!.User!.Attendances!.Where(a => attendanceCollections.ContainsKey(a.AttendanceCollectionID))
                .Select(a => new StudentAttendancesDTO()
                {
                    AttendanceId = a.AttendanceID,
                    BonusPoints = a.BonusPoints,
                    WasPresent = a.IsPresent,
                    UpdatedOn = a.UpdatedOn.ToString(Constants.DateFormat),
                    CourseType = attendanceCollections[a.AttendanceCollectionID].ToString()
                }).ToArray();
        }
    }
}
