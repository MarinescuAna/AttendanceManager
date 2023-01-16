using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Entities;
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
            if (await unitOfWork.DocumentRepository.GetAsync(d => d.Title == request.Title && d.UserID == request.Email && d.EnrollmentYear == int.Parse(request.EnrollmentYear) && !d.IsDeleted && d.SpecializationID.ToString() == request.SpecializationId) != null)
            {
                throw new AlreadyExistsException("Department", request.Title);
            }

            var newDocument = new Domain.Entities.Document
            {
                DocumentId = Guid.NewGuid(),
                CourseID = Guid.Parse(request.CourseId),
                CreationDate= DateTime.Now,
                EnrollmentYear = int.Parse(request.EnrollmentYear),
                IsDeleted = false,
                MaxNoLaboratories = int.Parse(request.MaxNoLaboratories),
                MaxNoLessons = int.Parse(request.MaxNoLessons),
                MaxNoSeminaries = int.Parse(request.MaxNoSeminaries),
                SpecializationID = Guid.Parse(request.SpecializationId),
                Title= request.Title,
                UserID = request.Email,
                UpdateDate= DateTime.Now
            };
            
            foreach(var user in request.StudentIds)
            {
                unitOfWork.UserDocumentRepository.AddAsync(new UserDocument
                {
                    UserDocumentId = Guid.NewGuid(),
                    DocumentID=newDocument.DocumentId,
                    UserID = user
                });
            }

            unitOfWork.DocumentRepository.AddAsync(newDocument);

            return await unitOfWork.CommitAsync();
        }
    }
}
