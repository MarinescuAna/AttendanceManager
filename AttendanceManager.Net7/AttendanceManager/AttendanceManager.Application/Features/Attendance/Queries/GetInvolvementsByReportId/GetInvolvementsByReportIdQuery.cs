
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Queries.GetInvolvementsByReportId
{
    public sealed class GetInvolvementsByReportIdQuery : IRequest<List<InvolvementVm>>
    {
        public int? CollectionId { get; init; }
        public string? UserEmail { get; init; }
        public bool OnlyPresent { get; init; } = false;
        public bool UseCode { get; init; } = false;
    }

    public sealed class GetInvolvementsByReportIdQueryHandler : BaseDocumentFeature, IRequestHandler<GetInvolvementsByReportIdQuery, List<InvolvementVm>>
    {
        public GetInvolvementsByReportIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public Task<List<InvolvementVm>> Handle(GetInvolvementsByReportIdQuery request, CancellationToken cancellationToken)
        {
            if (currentDocument == null)
            {
                throw new NoContentException(ErrorMessages.NoContentReportBaseMessage);
            }

            //get all involvements accoring with the filter
            var involvementsQuery = new List<Domain.Entities.Attendance>();


            if (request.CollectionId != -1 && string.IsNullOrEmpty(request.UserEmail))
            {
                //get all the involvement that have the collection id equal with the passed one
                involvementsQuery = unitOfWork.AttendanceRepository.ListAll()
                    .Where(a => a.AttendanceCollectionID == request.CollectionId).ToList();
            }
            else
            {
                //get all the involvement that have the email equal with the passed one
                if (request.CollectionId == -1 && !string.IsNullOrEmpty(request.UserEmail))
                {
                    involvementsQuery = unitOfWork.AttendanceRepository.ListAll()
                        .Where(a => a.UserID.Equals(request.UserEmail)).ToList();
                }
                else
                {
                    //get all the involvements that have the report id equal with the current one
                    involvementsQuery = unitOfWork.AttendanceRepository.ListAll().ToList()
                        .Where(a => attendanceCollectionsType != null && attendanceCollectionsType.ContainsKey(a.AttendanceCollectionID)).ToList();
                }
            }

            if (request.OnlyPresent)
            {
                //get only the involvements where the present is set to true 
                involvementsQuery = involvementsQuery.Where(a => a.IsPresent).ToList();
            }
            
            if (involvementsQuery.Count == 0)
            {
                return Task.FromResult(new List<InvolvementVm>());
            }

            //get all the students enrolled into the current report
            var reportStudents = documentMembers!.Where(s => s!.User!.Role == Role.Student);
            var results = new List<InvolvementVm>();

            foreach (var student in reportStudents)
            {
                results.AddRange(involvementsQuery.Where(i => i.UserID.Equals(student.UserID))
                    .Select(i => new InvolvementVm
                    {
                        InvolvementId = i.AttendanceID,
                        ActivityType = attendanceCollectionsType![i.AttendanceCollectionID],
                        BonusPoints = i.BonusPoints,
                        IsPresent = i.IsPresent,
                        Student = request.UseCode ? student.User!.Code : student.User!.FullName,
                        CollectionId = i.AttendanceCollectionID,
                        UpdateOn = i.UpdatedOn.ToString(Constants.ShortDateFormat),
                        Email = request.UseCode ? string.Empty : i.UserID
                    }));
            }

            return Task.FromResult(results);
        }
    }
}
