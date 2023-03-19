using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Shared;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Queries.GetDocumentById
{
    public sealed class GetDocumentByIdQueryHandler : BaseFeature, IRequestHandler<GetDocumentByIdQuery, DocumentInfoDto>
    {
        public GetDocumentByIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<DocumentInfoDto> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            var currentDocument = await unitOfWork.DocumentRepository.GetDocumentByIdAsync(request.Id)
                ?? throw new NotFoundException("Document", request.Id);
            var collectionAttendances = await unitOfWork.AttendanceCollectionRepository.GetAttendanceCollectionsByDocumentIdAsync(request.Id);
            var documentMembers = await unitOfWork.DocumentMemberRepository.GetDocumentMembersBtDocumentIdAsync(request.Id);

            return new DocumentInfoDto
            {
                CourseId = currentDocument.CourseID,
                CourseName = currentDocument.Course!.Name,
                CreationDate = currentDocument.CreatedOn.ToString(Constants.DateFormat),
                DocumentId = currentDocument.DocumentId,
                EnrollmentYear= currentDocument.EnrollmentYear,
                MaxNoLaboratories = currentDocument.MaxNoLaboratories,
                MaxNoLessons= currentDocument.MaxNoLessons,
                MaxNoSeminaries= currentDocument.MaxNoSeminaries,
                SpecializationId = currentDocument.Course!.UserSpecializationID,
                SpecializationName = currentDocument.Course!.UserSpecialization!.Specialization!.Name,
                Title = currentDocument.Title,
                UpdateDate = currentDocument.UpdatedOn.ToString(Constants.DateFormat),
                NoLaboratories = collectionAttendances.Count == 0? 0: collectionAttendances.Where(ca=>ca.CourseType == Domain.Enums.CourseType.Laboratory).Count(),
                NoLessons = collectionAttendances.Count == 0 ? 0 : collectionAttendances.Where(ca=>ca.CourseType == Domain.Enums.CourseType.Lesson).Count(),
                NoSeminaries = collectionAttendances.Count == 0 ? 0 : collectionAttendances.Where(ca=>ca.CourseType == Domain.Enums.CourseType.Seminary).Count(),
                CreatedBy = currentDocument.Course!.UserSpecialization!.User!.FullName,
                AttendanceCollections = mapper.Map < AttendanceCollectionDto[]>(currentDocument.AttendanceCollections!.OrderByDescending(d => d.HeldOn)),
                DocumentMembers = mapper.Map<DocumentMembersDto[]>(documentMembers)
            };
         }
    }
}
