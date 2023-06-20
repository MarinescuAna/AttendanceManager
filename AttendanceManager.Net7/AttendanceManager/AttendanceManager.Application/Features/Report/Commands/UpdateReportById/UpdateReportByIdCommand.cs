using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.Report.Commands.UpdateReportById
{
    public sealed class UpdateReportByIdCommand : IRequest<bool>
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

    public sealed class UpdateReportByIdCommandHandler : IRequestHandler<UpdateReportByIdCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReportSingleton _currentReport;
        public UpdateReportByIdCommandHandler(IUnitOfWork unitOfWork, IReportSingleton reportSingleton)
        {
            _unitOfWork = unitOfWork;
            _currentReport = reportSingleton;

            if (_currentReport.CurrentReportInfo == null)
            {
                throw new Exceptions.NotImplementedException(ErrorMessages.NoContentReportBaseMessage);
            }
        }

        public async Task<bool> Handle(UpdateReportByIdCommand request, CancellationToken cancellationToken)
        {
            var report = await _unitOfWork.ReportRepository.GetAsync(d => d.ReportID == request.DocumentId)
                ?? throw new NotFoundException("The report can't be found. Please, try again!");

            report.UpdatedOn = DateTime.Now;
            if (report.AttendanceImportance != request.AttendanceImportance)
            {
                report.AttendanceImportance = request.AttendanceImportance;
            }
            if (report.BonusPointsImportance != request.BonusPointsImportance)
            {
                report.BonusPointsImportance = request.BonusPointsImportance;
            }
            if (!report.Title.Equals(request.Title))
            {
                report.Title = request.Title;
            }
            if (report.MaxNoLaboratories != request.NoLaboratories)
            {
                report.MaxNoLaboratories = request.NoLaboratories;
            }
            if (report.MaxNoLessons != request.NoLessons)
            {
                report.MaxNoLessons = request.NoLessons;
            }
            if (report.MaxNoSeminaries != request.NoSeminaries)
            {
                report.MaxNoSeminaries = request.NoSeminaries;
            }
            if (report.CourseID != request.CourseId)
            {
                report.CourseID = request.CourseId;
            }

            _unitOfWork.ReportRepository.Update(report);

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            _currentReport.UpdateReport(report);

            return true;
        }
    }
}
