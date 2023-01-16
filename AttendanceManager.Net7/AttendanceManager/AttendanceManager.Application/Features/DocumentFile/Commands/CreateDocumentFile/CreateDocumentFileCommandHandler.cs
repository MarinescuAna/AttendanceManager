using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.DocumentFile.Commands.CreateDocumentFile
{
    public sealed class CreateDocumentFileCommandHandler :BaseFeature, IRequestHandler<CreateDocumentFileCommand, bool>
    {
        public CreateDocumentFileCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<bool> Handle(CreateDocumentFileCommand request, CancellationToken cancellationToken)
        {
            var documentFile = new Domain.Entities.DocumentFile
            {
                DocumentFileID = Guid.NewGuid(),
                DocumentID = Guid.Parse(request.DocumentId),
                ActivityTime = DateTime.Parse(request.ActivityDateTime),
                CourseType = (CourseType)Enum.Parse(typeof(CourseType), request.CourseType)
            };

            unitOfWork.DocumentFileRepository.AddAsync(documentFile);

            return await unitOfWork.CommitAsync();
        }
    }
}
