using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Core.Shared;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Commands.CreateDocument
{
    public sealed class CreateDocumentCommandHandler : BaseFeature, IRequestHandler<CreateDocumentCommand, InsertDocumentDto>
    {
        public CreateDocumentCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<InsertDocumentDto> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
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
            if(!await unitOfWork.CommitAsync(request.StudentIds.Length))
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            return new()
            {
                UpdatedOn = newDocument.UpdatedOn.ToString(Constants.ShortDateFormat),
                DocumentId = newDocument.DocumentId
            };
        }
    }
}
