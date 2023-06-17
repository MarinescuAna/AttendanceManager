using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Queries.GetReportById
{
    public sealed class GetReportByIdQuery : IRequest<ReportVm>
    {
        public required int Id { get; init; }
        public required Role Role { get; init; }
        public required string UserId { get; init; }
    }

    public sealed class GetDocumentByIdQueryHandler : IRequestHandler<GetReportByIdQuery, ReportVm>
    {
        private readonly IReportSingleton _currentReportService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetDocumentByIdQueryHandler(IUnitOfWork unit, IMapper mapper, IReportSingleton currentReportService)
        {
            _mapper = mapper;
            _unitOfWork = unit;
            _currentReportService = currentReportService;
        }

        public async Task<ReportVm> Handle(GetReportByIdQuery request, CancellationToken cancellationToken)
        {
            // get current document, which includes Collection
            var currentDocument = await _unitOfWork.DocumentRepository.GetDocumentByIdAsync(request.Id)
                ?? throw new NotFoundException("The document cannot be found!");

            //get the current document
            var test = _unitOfWork.DocumentMemberRepository.ListAll().Where(d=>d.DocumentID==request.Id);
            var documentMembers = await _unitOfWork.DocumentMemberRepository.GetDocumentMembersByDocumentIdAndRoleAsync(request.Id, null);
            _currentReportService.InitializeReport(currentDocument!,documentMembers);

            return new ReportVm
            {
                CourseId = currentDocument!.CourseID,
                CourseName = currentDocument.Course!.Name,
                CreationDate = currentDocument.CreatedOn.ToString(Constants.DateFormat),
                ReportId = currentDocument.DocumentId,
                EnrollmentYear = currentDocument.EnrollmentYear,
                MaxNoLaboratories = currentDocument.MaxNoLaboratories,
                MaxNoLessons = currentDocument.MaxNoLessons,
                MaxNoSeminaries = currentDocument.MaxNoSeminaries,
                SpecializationId = currentDocument.Course!.UserSpecializationID,
                SpecializationName = currentDocument.Course!.UserSpecialization!.Specialization!.Name,
                Title = currentDocument.Title,
                UpdatedOn = currentDocument.UpdatedOn.ToString(Constants.DateFormat),
                NoLaboratories = _currentReportService.ReportCollectionTypes.Count == 0 ?
                    0 : _currentReportService.ReportCollectionTypes.Where(ca => ca.Value == CourseType.Laboratory).Count(),
                NoLessons = _currentReportService.ReportCollectionTypes.Count == 0 ?
                    0 : _currentReportService.ReportCollectionTypes.Where(ca => ca.Value == CourseType.Lecture).Count(),
                NoSeminaries = _currentReportService.ReportCollectionTypes.Count == 0 ?
                    0 : _currentReportService.ReportCollectionTypes.Where(ca => ca.Value == CourseType.Seminary).Count(),
                CreatedBy = currentDocument.Course!.UserSpecialization!.User!.FullName,
                Collections = _mapper.Map<CollectionDto[]>(currentDocument.Collections!.OrderBy(d => d.HeldOn)),
                Members = _mapper.Map<MembersDto[]>(request.Role == Role.Teacher ?
                    documentMembers.Where(u => u.User!.Role == Role.Teacher) : documentMembers.Where(u => u.User!.Role == Role.Student)),
                AttendanceImportance = currentDocument.AttendanceImportance,
                BonusPointsImportance = currentDocument.BonusPointsImportance,
                NumberOfStudents = documentMembers.Count(u => u.User!.Role == Role.Student),
                IsCreator = request.Role == Role.Student ? false : currentDocument.Course!.UserSpecialization!.UserID.Equals(request.UserId)
            };
        }
    }
}
