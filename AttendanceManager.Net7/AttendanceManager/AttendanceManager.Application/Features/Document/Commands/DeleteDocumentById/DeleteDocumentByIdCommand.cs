using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Commands.DeleteDocumentById
{
    public sealed class DeleteDocumentByIdCommand : IRequest<bool>
    {
        public int? ReportId { get; init; }
    }
    public sealed class DeleteDocumentByIdCommandHandler : IRequestHandler<DeleteDocumentByIdCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReportSingleton _currentReport;
        public DeleteDocumentByIdCommandHandler(IUnitOfWork unit, IReportSingleton reportSingleton)
        {
            _unitOfWork = unit;
            _currentReport = reportSingleton;

            //don't check if the current document dosen't exist, because is ok to not
        }

        public async Task<bool> Handle(DeleteDocumentByIdCommand request, CancellationToken cancellationToken)
        {
            var reportId = (int)(request.ReportId == null ? _currentReport.CurrentReportInfo.ReportId : request.ReportId);

            // get the document
            var document = await _unitOfWork.DocumentRepository.GetAsync(d => d.DocumentId == reportId);

            if (document == null && request.ReportId != null)
            {
                //this is deleted
                return true;
            }

            if (document == null && request.ReportId == null)
            {
                //this is deleted from current document section
                throw new NotFoundException("The document was not found, so it cannot be deleted.");
            }

            //delete document members
            _unitOfWork.MemberRepository.DeleteMembersByReportId(reportId);

            //delete collections and attedances
            _unitOfWork.CollectionRepository.DeleteCollectionsByReportId(reportId);

            //delete custom badges and rewards
            _unitOfWork.RewardRepository.DeleteRewardsByReportId(reportId);
            _unitOfWork.BadgeRepository.DeleteCustomBadgesByReportId(reportId);

            //delete document
            _unitOfWork.DocumentRepository.Delete(document!);

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            return true;
        }
    }
}
