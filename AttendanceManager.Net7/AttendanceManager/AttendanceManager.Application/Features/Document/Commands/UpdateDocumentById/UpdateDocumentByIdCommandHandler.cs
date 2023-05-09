using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Commands.UpdateDocumentById
{
    public sealed class UpdateDocumentByIdCommandHandler : BaseFeature, IRequestHandler<UpdateDocumentByIdCommand, bool>
    {
        public UpdateDocumentByIdCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<bool> Handle(UpdateDocumentByIdCommand request, CancellationToken cancellationToken)
        {
            var document = await unitOfWork.DocumentRepository.GetAsync(d => d.DocumentId == request.DocumentId && !d.IsDeleted)
                ?? throw new NotFoundException("The document can't be found. Please, try again!");

            document.UpdatedOn = DateTime.Now;
            if (document.AttendanceImportance != request.AttendanceImportance)
            {
                document.AttendanceImportance = request.AttendanceImportance;
            }
            if (document.BonusPointsImportance != request.BonusPointsImportance)
            {
                document.BonusPointsImportance = request.BonusPointsImportance;
            }
            if (!document.Title.Equals(request.Title))
            {
                document.Title = request.Title;
            }
            if (document.MaxNoLaboratories != request.NoLaboratories)
            {
                document.MaxNoLaboratories = request.NoLaboratories;
            }
            if (document.MaxNoLessons != request.NoLessons)
            {
                document.MaxNoLessons = request.NoLessons;
            }
            if (document.MaxNoSeminaries != request.NoSeminaries)
            {
                document.MaxNoSeminaries = request.NoSeminaries;
            }
            if (document.CourseID != request.CourseId)
            {
                document.CourseID = request.CourseId;
            }

            unitOfWork.DocumentRepository.Update(document);

            return await unitOfWork.CommitAsync();
        }
    }
}
