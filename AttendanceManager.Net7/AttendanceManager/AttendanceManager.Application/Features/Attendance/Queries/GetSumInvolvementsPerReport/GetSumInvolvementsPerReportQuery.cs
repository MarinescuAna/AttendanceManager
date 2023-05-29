﻿using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Queries.GetSumInvolvementsPerReport
{
    public sealed class GetSumInvolvementsPerReportQuery : IRequest<List<InvolvementsSumVm>>
    {
    }
    public sealed class GetSumInvolvementsPerReportQueryHandler : IRequestHandler<GetSumInvolvementsPerReportQuery, List<InvolvementsSumVm>>
    {
        private readonly IReportSingleton _currentReport;
        private readonly IUnitOfWork _unitOfWork;
        public GetSumInvolvementsPerReportQueryHandler(IUnitOfWork unit, IReportSingleton reportSingleton)
        {
            _unitOfWork = unit;
            _currentReport = reportSingleton;

            if (_currentReport.CurrentReportInfo == null)
            {
                throw new NotImplementedException(ErrorMessages.NoContentReportBaseMessage);
            }
        }

        public async Task<List<InvolvementsSumVm>> Handle(GetSumInvolvementsPerReportQuery request, CancellationToken cancellationToken)
        {
            var students = await _unitOfWork.DocumentMemberRepository.GetDocumentMembersByDocumentIdAndRoleAsync(_currentReport.CurrentReportInfo.ReportId, Role.Student);
            var involvements = new List<InvolvementsSumVm>();

            // the user contains the attendances, but those are also from other document, so we will get only the attendances related to the collections from dictionary
            for (var i = 0; i < students.Count; i++)
            {
                var studentInvolvments = students[i].User!.Attendances!
                        .Where(a => a.IsPresent)
                        .Where(a => _currentReport.ReportCollectionTypes.ContainsKey(a.AttendanceCollectionID));
                involvements.Add(new()
                {
                    UserId = students[i].UserID,
                    UserName = students[i].User!.FullName,
                    Code = students[i].User!.Code,
                    BonusPoints = students[i].User?.Attendances == null ? 0 : studentInvolvments.Sum(a => a.BonusPoints),
                    CourseAttendances = students[i].User?.Attendances == null ? 0 : studentInvolvments.Count(a => _currentReport.ReportCollectionTypes[a.AttendanceCollectionID] == CourseType.Lecture),
                    LaboratoryAttendances = students[i].User?.Attendances == null ? 0 : studentInvolvments.Count(a => _currentReport.ReportCollectionTypes[a.AttendanceCollectionID] == CourseType.Laboratory),
                    SeminaryAttendances = students[i].User?.Attendances == null ? 0 : studentInvolvments.Count(a => _currentReport.ReportCollectionTypes[a.AttendanceCollectionID] == CourseType.Seminary),
                });
            }

            return involvements;
        }
    }
}
