
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Queries.GetInvolvementsByReportId
{
    public sealed class GetInvolvementsByReportIdQuery : IRequest<List<InvolvementDto>>
    {
    }

    public sealed class GetInvolvementsByReportIdQueryHandler : BaseDocumentFeature, IRequestHandler<GetInvolvementsByReportIdQuery, List<InvolvementDto>>
    {
        public GetInvolvementsByReportIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public Task<List<InvolvementDto>> Handle(GetInvolvementsByReportIdQuery request, CancellationToken cancellationToken)
        {
            var involvements = new List<InvolvementDto>();
            //get all the students enrolled into the current report
            var reportStudents = documentMembers!.Where(s => s!.User!.Role == Role.Student);

            if (reportStudents.Count() == 0)
            {
                return Task.FromResult(involvements);
            }

            foreach (var student in reportStudents)
            {
                // for each student, get the attendances needed because those can contain the attendances from other departments
                var studentAttendances = student.User?.Attendances?
                    .Where(a => a.IsPresent)
                    .Where(a => attendanceCollectionsType!.ContainsKey(a.AttendanceCollectionID));

                if (studentAttendances?.Count() != 0)
                {
                    involvements.AddRange(studentAttendances!
                        .Select(a => new InvolvementDto()
                        {
                            ActivityType = attendanceCollectionsType![a.AttendanceCollectionID],
                            CollectionId = a.AttendanceCollectionID,
                            BonusPoints = a.BonusPoints,
                            InvolvementId = a.AttendanceID,
                            StudentEmail = a.UserID
                        }).ToList());
                }
            }

            return Task.FromResult(involvements);
        }
    }
}
