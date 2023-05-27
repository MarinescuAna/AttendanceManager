using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Dtos;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Queries.GetDocumentById
{
    public sealed class GetDocumentByIdQuery : IRequest<DocumentInfoDto>
    {
        public required int Id { get; init; }
        public required Role Role { get; init; }
        public required string UserId { get; init; }
    }

    public sealed class GetDocumentByIdQueryHandler : BaseDocumentFeature, IRequestHandler<GetDocumentByIdQuery, DocumentInfoDto>
    {
        public GetDocumentByIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<DocumentInfoDto> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            //get the current document
            await this.InitialiazeDocumentAsync(request.Id);

            return new DocumentInfoDto
            {
                CourseId = currentDocument!.CourseID,
                CourseName = currentDocument.Course!.Name,
                CreationDate = currentDocument.CreatedOn.ToString(Constants.DateFormat),
                DocumentId = currentDocument.DocumentId,
                EnrollmentYear = currentDocument.EnrollmentYear,
                MaxNoLaboratories = currentDocument.MaxNoLaboratories,
                MaxNoLessons = currentDocument.MaxNoLessons,
                MaxNoSeminaries = currentDocument.MaxNoSeminaries,
                SpecializationId = currentDocument.Course!.UserSpecializationID,
                SpecializationName = currentDocument.Course!.UserSpecialization!.Specialization!.Name,
                Title = currentDocument.Title,
                UpdatedOn = currentDocument.UpdatedOn.ToString(Constants.DateFormat),
                NoLaboratories = attendanceCollectionsType!.Count == 0 ? 0 : attendanceCollectionsType.Where(ca => ca.Value == CourseType.Laboratory).Count(),
                NoLessons = attendanceCollectionsType.Count == 0 ? 0 : attendanceCollectionsType.Where(ca => ca.Value == CourseType.Lecture).Count(),
                NoSeminaries = attendanceCollectionsType.Count == 0 ? 0 : attendanceCollectionsType.Where(ca => ca.Value == CourseType.Seminary).Count(),
                CreatedBy = currentDocument.Course!.UserSpecialization!.User!.FullName,
                AttendanceCollections = mapper.Map<AttendanceCollectionDto[]>(currentDocument.AttendanceCollections!.OrderBy(d => d.HeldOn)),
                DocumentMembers = mapper.Map<DocumentMembersDto[]>(request.Role == Role.Teacher ? documentMembers?.Where(u => u.User!.Role == Role.Teacher) : documentMembers),
                AttendanceImportance = currentDocument.AttendanceImportance,
                BonusPointsImportance = currentDocument.BonusPointsImportance,
                NumberOfStudents = documentMembers!.Count(u=>u.User!.Role == Role.Student)
            };
        }
    }
}
