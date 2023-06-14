using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Features.Document.Queries.GetDocuments;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Queries.GetCreatedDocumentsByEmail
{
    public sealed class GetDocumentsQuery : IRequest<IEnumerable<ReportVm>>
    {
        public required string Email { get; init; }
        public required Role Role { get; init; }
    }

    public sealed class GetDocumentsQueryHandler : IRequestHandler<GetDocumentsQuery, IEnumerable<ReportVm>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDocumentsQueryHandler(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        /*
         * Get documents according to the user email and role:
         * - Admin: get all the documents where the user has the role Creator or collaborator
         * - Student: get all the documents where the student has the role Member
         */
        public async Task<IEnumerable<ReportVm>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
        {
            var allDocuments = new List<ReportVm>();
            var documents = request.Role == Role.Student ?
                await _unitOfWork.DocumentRepository.GetUserDocumentsByExpressionAsync(u =>
                    u.DocumentMembers!.Any(m => m.UserID == request.Email)) :
                await _unitOfWork.DocumentRepository.GetUserDocumentsByExpressionAsync(u =>
                    (u.Course!.UserSpecialization!.UserID == request.Email || u.DocumentMembers!.Any(m => m.UserID == request.Email)));

            foreach (var document in documents.OrderByDescending(d => d.CreatedOn))
            {
                allDocuments.Add(new()
                {
                    IsCreator = request.Role == Role.Student ? false : document.Course!.UserSpecialization!.UserID.Equals(request.Email),
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
