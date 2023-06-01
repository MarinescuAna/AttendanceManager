using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.Document.Commands.CreateDocument
{
    public sealed class CreateDocumentCommand : IRequest<InsertDocumentVm>
    {
        public required string Title { get; init; }
        public required int EnrollmentYear { get; init; }
        public string? Email { get; set; }
        public required int MaxNoSeminaries { get; init; }
        public required int MaxNoLaboratories { get; init; }
        public required int MaxNoLessons { get; init; }
        public required int CourseId { get; init; }
        public required int SpecializationId { get; set; }
        public required int AttendanceImportance { get; init; }
        public required int BonusPointsImportance { get; init; }
        public required string[] StudentIds { get; set; }

    }

    public sealed class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, InsertDocumentVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateDocumentCommandHandler(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }

        public async Task<InsertDocumentVm> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
            var newReport = new Domain.Entities.Document
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
            _unitOfWork.DocumentRepository.AddAsync(newReport);
            await _unitOfWork.CommitAsync();

            // REMEMBER: the teacher that create the document can be access form course, so there is no reason to store it in the DocumentMember table

            // insert all the sudents into the document
            foreach (var user in request.StudentIds)
            {
                _unitOfWork.DocumentMemberRepository.AddAsync(new Domain.Entities.DocumentMember
                {
                    DocumentID = newReport.DocumentId,
                    UserID = user,
                });
            }

            // Save the members 
            if (!await _unitOfWork.CommitAsync(request.StudentIds.Length))
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            //send notifications to each user 
            foreach (var user in request.StudentIds)
            {
                _unitOfWork.NotificationRepository.AddAsync(new()
                {
                    Priority= Domain.Enums.NotificationPriority.Info,
                    UserID = user,
                    CreatedOn = DateTime.Now,
                    IsRead = false,
                    Message = string.Format(NotificationMessages.CreateReportNotification,newReport.Title,request.Email,newReport.CreatedOn.ToString(Constants.ShortDateFormat)),
                });
            }

            // Save the members 
            if (!await _unitOfWork.CommitAsync(request.StudentIds.Length))
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrong_CreateReportNotificationInsert_Error);
            }

            return new()
            {
                UpdatedOn = newReport.UpdatedOn.ToString(Constants.ShortDateFormat),
                DocumentId = newReport.DocumentId
            };
        }
    }
}
