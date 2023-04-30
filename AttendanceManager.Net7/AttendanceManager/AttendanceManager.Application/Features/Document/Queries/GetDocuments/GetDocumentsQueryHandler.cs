using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Dtos;
using AttendanceManager.Core.Shared;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Queries.GetCreatedDocumentsByEmail
{
    public sealed class GetDocumentsQueryHandler : BaseFeature, IRequestHandler<GetDocumentsQuery, IEnumerable<DocumentDto>>
    {
        public GetDocumentsQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }
        /*
         * Get documents according to the user email and role:
         * - Admin: get all the documents where the user has the role Creator or collaborator
         * - Student: get all the documents where the student has the role Member
         */
        public async Task<IEnumerable<DocumentDto>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
        {
            var allDocuments = new List<DocumentDto>();
            var documents = request.Role == Role.Student ?
                await unitOfWork.DocumentRepository.GetUserDocumentsByExpressionAsync(u =>
                    u.DocumentMembers!.Any(m => m.UserID == request.Email && m.Role == DocumentRole.Member) && !u.IsDeleted) :
                await unitOfWork.DocumentRepository.GetUserDocumentsByExpressionAsync(u => 
                    (u.Course!.UserSpecialization!.UserID == request.Email || u.DocumentMembers!.Any(m => m.UserID == request.Email && m.Role == DocumentRole.Collaborator)) && !u.IsDeleted);

            foreach (var document in documents.OrderByDescending(d => d.CreatedOn))
            {
                allDocuments.Add(new()
                {
                    IsCreator = request.Role == Role.Student ? false: document.Course!.UserSpecialization!.UserID.Equals(request.Email),
                    CourseName = document.Course!.Name,
                    DocumentId = document.DocumentId,
                    EnrollmentYear = document.EnrollmentYear,
                    SpecializationName = document.Course!.UserSpecialization!.Specialization!.Name,
                    Title = document.Title,
                    UpdatedOn = document.UpdatedOn.ToString(Constants.ShortDateFormat)
                });
            }

            return allDocuments;
        }

    }
}
