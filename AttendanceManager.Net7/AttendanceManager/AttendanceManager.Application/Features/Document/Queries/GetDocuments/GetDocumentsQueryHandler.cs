using AttendanceManager.Application.Contracts.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Queries.GetCreatedDocumentsByEmail
{
    public sealed class GetDocumentsQueryHandler : BaseFeature, IRequestHandler<GetDocumentsQuery, List<DocumentDto>>
    {
        public GetDocumentsQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }
        /*
         * Get documents according to the user email and role:
         * - Admin: get all the documents where the user has the role Creator
         * - Student: get all the documents where the student has the role Member
         */
        public async Task<List<DocumentDto>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
        {
            var documents = 
                await unitOfWork.DocumentRepository.GetUserDocumentsByExpressionAsync(u =>
                    (request.Role == Domain.Enums.Role.Admin && u.DocumentMembers!.Any(m => m.UserID == request.Email && m.Document!.Course!.UserSpecialization!.UserID.Equals(request.Email))) ||
                    (request.Role == Domain.Enums.Role.Student && u.DocumentMembers!.Any(m => m.UserID == request.Email && m.Role == Domain.Enums.DocumentRole.Member))
                );

            return mapper.Map<List<DocumentDto>>(documents.OrderByDescending(d => d.UpdatedOn));
        }
    }
}
