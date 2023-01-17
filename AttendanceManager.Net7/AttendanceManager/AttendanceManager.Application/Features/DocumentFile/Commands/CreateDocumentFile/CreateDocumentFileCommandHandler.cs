using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Shared;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.DocumentFile.Commands.CreateDocumentFile
{
    public sealed class CreateDocumentFileCommandHandler :BaseFeature, IRequestHandler<CreateDocumentFileCommand, Guid>
    {
        public CreateDocumentFileCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<Guid> Handle(CreateDocumentFileCommand request, CancellationToken cancellationToken)
        {
            var documentFile = new Domain.Entities.DocumentFile
            {
                DocumentFileID = Guid.NewGuid(),
                DocumentID = Guid.Parse(request.DocumentId),
                ActivityTime = DateTime.Parse(request.ActivityDateTime),
                CourseType = (CourseType)Enum.Parse(typeof(CourseType), request.CourseType)
            };

            unitOfWork.DocumentFileRepository.AddAsync(documentFile);

            if (!await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(Constants.SomethingWentWrongMessage);
            }

            // Return the created department [the department id is mandatory]
            return documentFile.DocumentFileID;
        }
    }
}
