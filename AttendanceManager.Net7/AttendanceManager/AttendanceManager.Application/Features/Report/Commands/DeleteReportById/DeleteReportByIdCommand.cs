using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.Report.Commands.DeleteDocumentById
{
    public sealed class DeleteReportByIdCommand : IRequest<bool>
    {
        public int? ReportId { get; init; }
    }
    public sealed class DeleteDocumentByIdCommandHandler : IRequestHandler<DeleteReportByIdCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReportSingleton _currentReport;
        public DeleteDocumentByIdCommandHandler(IUnitOfWork unit, IReportSingleton reportSingleton)
        {
            _unitOfWork = unit;
            _currentReport = reportSingleton;

            //don't check if the current report dosen't exist, because is ok to not
        }

        public async Task<bool> Handle(DeleteReportByIdCommand request, CancellationToken cancellationToken)
        {
            var reportId = (int)(request.ReportId == null ? _currentReport.CurrentReportInfo.ReportId : request.ReportId);

            // get the report
            var report = await _unitOfWork.ReportRepository.GetAsync(d => d.ReportID == reportId);

            if (report == null && request.ReportId != null)
            {
                //this is deleted
                return true;
            }

            if (report == null && request.ReportId == null)
            {
                //this is deleted from current report section
                throw new NotFoundException("The report was not found, so it cannot be deleted.");
            }

            //delete members
            _unitOfWork.MemberRepository.DeleteMembersByReportId(reportId);

            //delete collections and attedances
            _unitOfWork.CollectionRepository.DeleteCollectionsByReportId(reportId);

            //delete custom badges and rewards
            _unitOfWork.RewardRepository.DeleteRewardsByReportId(reportId);
            _unitOfWork.BadgeRepository.DeleteCustomBadgesByReportId(reportId);

            //delete report
            _unitOfWork.ReportRepository.Delete(report!);

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            return true;
        }
    }
}
