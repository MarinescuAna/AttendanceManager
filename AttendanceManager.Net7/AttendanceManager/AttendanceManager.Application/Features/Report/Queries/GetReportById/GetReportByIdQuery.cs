using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Report.Queries.GetReportById
{
    public sealed class GetReportByIdQuery : IRequest<ReportVm>
    {
        public required int Id { get; init; }
        public required Role Role { get; init; }
        public required string UserId { get; init; }
    }

    public sealed class GetReportByIdQueryHandler : IRequestHandler<GetReportByIdQuery, ReportVm>
    {
        private readonly IReportSingleton _currentReportService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetReportByIdQueryHandler(IUnitOfWork unit, IMapper mapper, IReportSingleton currentReportService)
        {
            _mapper = mapper;
            _unitOfWork = unit;
            _currentReportService = currentReportService;
        }

        public async Task<ReportVm> Handle(GetReportByIdQuery request, CancellationToken cancellationToken)
        {
            // get current report, which includes Collection
            var currentReport = await _unitOfWork.ReportRepository.GetReportByIdAsync(request.Id)
                ?? throw new NotFoundException("The report cannot be found!");

            //get the current report
            var test = _unitOfWork.MemberRepository.ListAll().Where(d=>d.ReportID==request.Id);
            var members = await _unitOfWork.MemberRepository.GetMembersByReportIdAndRoleAsync(request.Id, null);
            _currentReportService.InitializeReport(currentReport!,members);

            return new ReportVm
            {
                CourseId = currentReport!.CourseID,
                CourseName = currentReport.Course!.Name,
                CreationDate = currentReport.CreatedOn.ToString(Constants.DateFormat),
                ReportId = currentReport.ReportID,
                EnrollmentYear = currentReport.EnrollmentYear,
                MaxNoLaboratories = currentReport.MaxNoLaboratories,
                MaxNoLessons = currentReport.MaxNoLessons,
                MaxNoSeminaries = currentReport.MaxNoSeminaries,
                SpecializationId = currentReport.Course!.UserSpecializationID,
                SpecializationName = currentReport.Course!.UserSpecialization!.Specialization!.Name,
                Title = currentReport.Title,
                UpdatedOn = currentReport.UpdatedOn.ToString(Constants.DateFormat),
                NoLaboratories = _currentReportService.ReportCollectionTypes.Count == 0 ?
                    0 : _currentReportService.ReportCollectionTypes.Where(ca => ca.Value == ActivityType.Laboratory).Count(),
                NoLessons = _currentReportService.ReportCollectionTypes.Count == 0 ?
                    0 : _currentReportService.ReportCollectionTypes.Where(ca => ca.Value == ActivityType.Lecture).Count(),
                NoSeminaries = _currentReportService.ReportCollectionTypes.Count == 0 ?
                    0 : _currentReportService.ReportCollectionTypes.Where(ca => ca.Value == ActivityType.Seminary).Count(),
                CreatedBy = currentReport.Course!.UserSpecialization!.User!.FullName,
                Collections = _mapper.Map<CollectionDto[]>(currentReport.Collections!.OrderBy(d => d.HeldOn)),
                Members = _mapper.Map<MembersDto[]>(request.Role == Role.Teacher ?
                    members.Where(u => u.User!.Role == Role.Teacher) : members.Where(u => u.User!.Role == Role.Student)),
                AttendanceImportance = currentReport.AttendanceImportance,
                BonusPointsImportance = currentReport.BonusPointsImportance,
                NumberOfStudents = members.Count(u => u.User!.Role == Role.Student),
                IsCreator = request.Role == Role.Student ? false : currentReport.Course!.UserSpecialization!.UserID.Equals(request.UserId)
            };
        }
    }
}
