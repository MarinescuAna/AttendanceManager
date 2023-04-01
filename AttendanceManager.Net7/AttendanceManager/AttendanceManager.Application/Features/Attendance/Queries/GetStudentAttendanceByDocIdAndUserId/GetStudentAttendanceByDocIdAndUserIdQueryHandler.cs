using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Core.Shared;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Queries.GetStudentAttendanceByDocIdAndUserId
{
    public sealed class GetStudentAttendanceByDocIdAndUserIdQueryHandler : BaseDocumentFeature, IRequestHandler<GetStudentAttendanceByDocIdAndUserIdQuery, StudentAttendancesDTO[]>
    {
        public GetStudentAttendanceByDocIdAndUserIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<StudentAttendancesDTO[]> Handle(GetStudentAttendanceByDocIdAndUserIdQuery request, CancellationToken cancellationToken)
        {
            var documentMember = await unitOfWork.DocumentMemberRepository.GetMemberByDocumentIdAndUserIdAsync(request.DocumentId, request.UserId)
                ?? throw new BadRequestException("No data available.");

            return documentMember!.User!.Attendances!.Where(a => attendanceCollectionsType!.ContainsKey(a.AttendanceCollectionID))
                .Select(a => new StudentAttendancesDTO()
                {
                    AttendanceId = a.AttendanceID,
                    BonusPoints = a.BonusPoints,
                    WasPresent = a.IsPresent,
                    UpdatedOn = a.UpdatedOn.ToString(Constants.DateFormat),
                    CourseType = attendanceCollectionsType![a.AttendanceCollectionID].ToString()
                }).ToArray();
        }
    }
}
