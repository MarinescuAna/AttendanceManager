using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Features.Attendance.Queries.GetTotalAttendancesByDocumentId;
using AttendanceManager.Application.Shared;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Queries.GetStudentAttendanceByDocIdAndUserId
{
    public sealed class GetStudentAttendanceByDocIdAndUserIdQueryHandler : BaseFeature, IRequestHandler<GetStudentAttendanceByDocIdAndUserIdQuery, List<StudentAttendancesDTO>>
    {
        public GetStudentAttendanceByDocIdAndUserIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<List<StudentAttendancesDTO>> Handle(GetStudentAttendanceByDocIdAndUserIdQuery request, CancellationToken cancellationToken)
        {
            var totalAttendances = new List<StudentAttendancesDTO>();
            var documentMember = await unitOfWork.DocumentMemberRepository.GetMemberByDocumentIdAndUserIdAsync(request.DocumentId, request.UserId);
            var attendanceCollections = documentMember!.Document!.AttendanceCollections!.ToDictionary(ac => ac.AttendanceCollectionID, ac => ac.CourseType);

            foreach(var attendance in documentMember!.User!.Attendances!)
            {
                if (attendanceCollections.ContainsKey(attendance.AttendanceCollectionID))
                {
                    totalAttendances.Add(new()
                    {
                        AttendanceId = attendance.AttendanceID,
                        BonusPoints = attendance.BonusPoints,
                        WasPresent = attendance.IsPresent,
                        UpdatedOn = attendance.UpdatedOn.ToString(Constants.DateFormat),
                        CourseType = attendanceCollections[attendance.AttendanceCollectionID].ToString()
                    });
                }
            }

            return totalAttendances;
        }
    }
}
