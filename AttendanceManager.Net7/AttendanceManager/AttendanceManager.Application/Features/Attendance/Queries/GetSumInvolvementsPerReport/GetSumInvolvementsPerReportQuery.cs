using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Queries.GetSumInvolvementsPerReport
{
    public sealed class GetSumInvolvementsPerReportQuery : IRequest<List<InvolvementsSumVm>>
    {
    }
    public sealed class GetSumInvolvementsPerReportQueryHandler : BaseDocumentFeature, IRequestHandler<GetSumInvolvementsPerReportQuery, List<InvolvementsSumVm>>
    {
        public GetSumInvolvementsPerReportQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public Task<List<InvolvementsSumVm>> Handle(GetSumInvolvementsPerReportQuery request, CancellationToken cancellationToken)
        {
            if (currentDocument == null)
            {
                throw new NoContentException(ErrorMessages.NoContentReportBaseMessage);
            }

            var students = documentMembers!.Where(s => s!.User!.Role == Role.Student).ToList();
            var involvements = new List<InvolvementsSumVm>();

            // the user contains the attendances, but those are also from other document, so we will get only the attendances related to the collections from dictionary
            for (var i = 0; i < students.Count; i++)
            {
                involvements.Add(new()
                {
                    UserId = students[i].UserID,
                    UserName = students[i].User!.FullName,
                    Code = students[i].User!.Code,
                    BonusPoints = students[i].User?.Attendances == null ? 0 : students[i].User!.Attendances!.Where(a => attendanceCollectionsType!.ContainsKey(a.AttendanceCollectionID)).Sum(a => a.BonusPoints),
                    CourseAttendances = students[i].User?.Attendances == null ? 0 : students[i].User!.Attendances!.Count(a => a.IsPresent && attendanceCollectionsType![a.AttendanceCollectionID] == CourseType.Lecture),
                    LaboratoryAttendances = students[i].User?.Attendances == null ? 0 : students[i].User!.Attendances!.Count(a => a.IsPresent && attendanceCollectionsType![a.AttendanceCollectionID] == CourseType.Laboratory),
                    SeminaryAttendances = students[i].User?.Attendances == null ? 0 : students[i].User!.Attendances!.Count(a => a.IsPresent && attendanceCollectionsType![a.AttendanceCollectionID] == CourseType.Seminary),
                });
            }

            return Task.FromResult(involvements);
        }
    }
}
