using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Commands.CreateDocument
{
    public sealed class CreateDocumentCommandHandler : BaseFeature, IRequestHandler<CreateDocumentCommand, bool>
    {
        public CreateDocumentCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<bool> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
            // Look for other documents with the same name, enrollmentyear, created by the same user for the same specialization
            if (await unitOfWork.DocumentRepository.GetAsync(d =>
                d.Title == request.Title &&
                d.CourseID == request.CourseId &&
                d.EnrollmentYear == request.EnrollmentYear &&
                !d.IsDeleted) != null)
            {
                throw new AlreadyExistsException("Document", request.Title);
            }

            var newDocument = new Domain.Entities.Document
            {
                CourseID = request.CourseId,
                CreatedOn = DateTime.Now,
                EnrollmentYear = request.EnrollmentYear,
                IsDeleted = false,
                MaxNoLaboratories = request.MaxNoLaboratories,
                MaxNoLessons = request.MaxNoLessons,
                MaxNoSeminaries = request.MaxNoSeminaries,
                Title = request.Title,
                UpdatedOn = DateTime.Now,
                AttendanceImportance = request.AttendanceImportance,
                BonusPointsImportance = request.BonusPointsImportance
            };
            // Save document first to can get the id
            unitOfWork.DocumentRepository.AddAsync(newDocument);
            await unitOfWork.CommitAsync();

            // REMEMBER: the teacher that create the document can be access form course, so there is no reason to store it in the DocumentMember table

            // insert all the sudents into the document
            foreach (var user in request.StudentIds)
            {
                unitOfWork.DocumentMemberRepository.AddAsync(new Domain.Entities.DocumentMember
                {
                    DocumentID = newDocument.DocumentId,
                    UserID = user,
                    Role = Domain.Enums.DocumentRole.Member
                });
            }

            // Save the members 
            return await unitOfWork.CommitAsync();
        }
    }
}
