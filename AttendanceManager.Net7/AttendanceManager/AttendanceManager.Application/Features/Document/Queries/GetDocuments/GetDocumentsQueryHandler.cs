using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Domain.Entities;
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
            if(request.Role == Domain.Enums.Role.Student)
            {
                return mapper.Map<IEnumerable<DocumentDto>>((await GetDocumentsForStudent(request.Email)).OrderByDescending(d => d.UpdatedOn));
            }
            
            var allDocuments = new List<DocumentDto>();            
            // get documents created by the user
            var documents = await unitOfWork.DocumentRepository.GetUserDocumentsByExpressionAsync(u =>
                    request.Role == Domain.Enums.Role.Teacher && u.Course!.UserSpecialization!.UserID.Equals(request.Email)
                );

            allDocuments.AddRange(mapper.Map<IEnumerable<DocumentDto>>(documents));
            allDocuments.ForEach(d => d.IsCreator = true);

            //get documents where the user is collaborator
            allDocuments.AddRange(mapper.Map<IEnumerable<DocumentDto>>(await unitOfWork.DocumentRepository.GetUserDocumentsByExpressionAsync(u =>
                    request.Role == Domain.Enums.Role.Teacher &&
                    u.DocumentMembers!.Any(m => m.UserID == request.Email && m.Role == Domain.Enums.DocumentRole.Collaborator))
                ));

            return allDocuments.OrderByDescending(d => d.UpdatedOn);
        }
        
        private async Task<IEnumerable<Domain.Entities.Document>> GetDocumentsForStudent(string email)
            => await unitOfWork.DocumentRepository.GetUserDocumentsByExpressionAsync(u =>
                u.DocumentMembers!.Any(m => m.UserID == email && m.Role == Domain.Enums.DocumentRole.Member));
    }
}
