using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Commands.UpdateDocumentById
{
    public sealed class UpdateDocumentByIdCommand : IRequest<bool>
    {
        public required int DocumentId { get; init; }
        public required string Title { get; init; }
        public required int NoSeminaries { get; init; }
        public required int NoLaboratories { get; init; }
        public required int NoLessons { get; init; }
        public required int CourseId { get; init; }
        public required int AttendanceImportance { get; init; }
        public required int BonusPointsImportance { get; init; }
    }

    public sealed class UpdateDocumentByIdCommandHandler : IRequestHandler<UpdateDocumentByIdCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReportSingleton _currentReport;
        public UpdateDocumentByIdCommandHandler(IUnitOfWork unitOfWork, IReportSingleton reportSingleton)
        {
            _unitOfWork = unitOfWork;
            _currentReport = reportSingleton;

            if (_currentReport.CurrentReportInfo == null)
            {
                throw new Exceptions.NotImplementedException(ErrorMessages.NoContentReportBaseMessage);
            }
        }

        public async Task<bool> Handle(UpdateDocumentByIdCommand request, CancellationToken cancellationToken)
        {
            var document = await _unitOfWork.DocumentRepository.GetAsync(d => d.DocumentId == request.DocumentId && !d.IsDeleted)
                ?? throw new NotFoundException("The document can't be found. Please, try again!");

            document.UpdatedOn = DateTime.Now;
            if (document.AttendanceImportance != request.AttendanceImportance)
            {
                document.AttendanceImportance = request.AttendanceImportance;
            }
            if (document.BonusPointsImportance != request.BonusPointsImportance)
            {
                document.BonusPointsImportance = request.BonusPointsImportance;
            }
            if (!document.Title.Equals(request.Title))
            {
                document.Title = request.Title;
            }
            if (document.MaxNoLaboratories != request.NoLaboratories)
            {
                document.MaxNoLaboratories = request.NoLaboratories;
            }
            if (document.MaxNoLessons != request.NoLessons)
            {
                document.MaxNoLessons = request.NoLessons;
            }
            if (document.MaxNoSeminaries != request.NoSeminaries)
            {
                document.MaxNoSeminaries = request.NoSeminaries;
            }
            if (document.CourseID != request.CourseId)
            {
                document.CourseID = request.CourseId;
            }

            _unitOfWork.DocumentRepository.Update(document);

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            _currentReport.UpdateReport(document);

            return true;
        }
    }
}
