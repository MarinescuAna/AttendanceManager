
using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
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

    public sealed class GetInvolvementsByReportIdQueryHandler : IRequestHandler<GetInvolvementsByReportIdQuery, List<InvolvementVm>>
    {
        private readonly IReportSingleton _currentReport;
        private readonly IUnitOfWork _unitOfWork;
        public GetInvolvementsByReportIdQueryHandler(IUnitOfWork unit, IReportSingleton reportSingleton)
        {
            _unitOfWork = unit;
            _currentReport = reportSingleton;

            if (_currentReport.CurrentReportInfo == null)
            {
                throw new Exceptions.NotImplementedException(ErrorMessages.NoContentReportBaseMessage);
            }
        }

        public async Task<List<InvolvementVm>> Handle(GetInvolvementsByReportIdQuery request, CancellationToken cancellationToken)
        {
            //NOTE: email and collection ID will never get passed together

            //get all the involvements for the requested user
            if (request.CollectionId == -1 && !string.IsNullOrEmpty(request.UserEmail))
            {
                var studentInvolvments = request.OnlyPresent ?
                    _unitOfWork.AttendanceRepository.ListAll().Where(a => a.UserID.Equals(request.UserEmail) && a.IsPresent).ToList() :
                    _unitOfWork.AttendanceRepository.ListAll().Where(a => a.UserID.Equals(request.UserEmail)).ToList();

                if (studentInvolvments.Count() == 0)
                {
                    return new List<InvolvementVm>();
                }

                var student = await _unitOfWork.UserRepository.GetAsync(s => s.Email.Equals(request.UserEmail));

                return studentInvolvments
                    .Where(i => i.UserID.Equals(request.UserEmail))
                    .Where(i => _currentReport.ReportCollectionTypes!.ContainsKey(i.AttendanceCollectionID))
                        .Select(i => new InvolvementVm
                        {
                            InvolvementId = i.AttendanceID,
                            ActivityType = _currentReport.ReportCollectionTypes![i.AttendanceCollectionID],
                            BonusPoints = i.BonusPoints,
                            IsPresent = i.IsPresent,
                            Student = request.UseCode ? student!.Code : student!.FullName,
                            CollectionId = i.AttendanceCollectionID,
                            UpdateOn = i.UpdatedOn.ToString(Constants.ShortDateFormat),
                            Email = request.UseCode ? string.Empty : i.UserID
                        }).ToList();
            }

            //get all the involvements for a collection
            var results = new List<InvolvementVm>();
            var involvements = Enumerable.Empty<Domain.Entities.Attendance>().AsQueryable();

            if (request.CollectionId != -1 && string.IsNullOrEmpty(request.UserEmail))
            {
                //get all the involvements that have the collection id equal with the passed one
                involvements = request.OnlyPresent ?
                    _unitOfWork.AttendanceRepository.ListAll().Where(a => a.AttendanceCollectionID == request.CollectionId && a.IsPresent) :
                    _unitOfWork.AttendanceRepository.ListAll().Where(a => a.AttendanceCollectionID == request.CollectionId);
            }
            else
            {
                //get all the involvements that have the report id equal with the current one
                involvements = request.OnlyPresent ?
                    _unitOfWork.AttendanceRepository.ListAll().Where(a => a.AttendanceCollection!.DocumentID== _currentReport.CurrentReportInfo.ReportId && a.IsPresent).AsQueryable() :
                    _unitOfWork.AttendanceRepository.ListAll().Where(a => a.AttendanceCollection!.DocumentID == _currentReport.CurrentReportInfo.ReportId).AsQueryable();
            }

            if (involvements.Count() == 0)
            {
                return results;
            }

            //get all the students enrolled into the current report
            var reportStudents = await _unitOfWork.DocumentMemberRepository.GetDocumentMembersByDocumentIdAndRoleAsync(_currentReport.CurrentReportInfo.ReportId, Role.Student);

            foreach (var student in reportStudents)
            {
                results.AddRange(involvements
                    .Where(i => i.UserID.Equals(student.UserID))
                    .Select(i => new InvolvementVm
                    {
                        InvolvementId = i.AttendanceID,
                        ActivityType = _currentReport.ReportCollectionTypes![i.AttendanceCollectionID],
                        BonusPoints = i.BonusPoints,
                        IsPresent = i.IsPresent,
                        Student = request.UseCode ? student.User!.Code : student.User!.FullName,
                        CollectionId = i.AttendanceCollectionID,
                        UpdateOn = i.UpdatedOn.ToString(Constants.ShortDateFormat),
                        Email = request.UseCode ? string.Empty : i.UserID
                    }));
            }

            return results;
        }
    }
}
