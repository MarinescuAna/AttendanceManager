using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.AttendanceCollection.Commands.CreateAttendanceCollection
{
    public sealed class CreateAttendanceCollectionCommand : IRequest<int>
    {
        public required int DocumentId { get; init; }
        public required string ActivityDateTime { get; init; }
        public required string CourseType { get; init; }
    }

    public sealed class CreateAttendanceCollectionCommandHandler : BaseFeature, IRequestHandler<CreateAttendanceCollectionCommand, int>
    {
        public CreateAttendanceCollectionCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<int> Handle(CreateAttendanceCollectionCommand request, CancellationToken cancellationToken)
        {
            var courseType = (CourseType)Enum.Parse(typeof(CourseType), request.CourseType);
            var maxOrder = unitOfWork.AttendanceCollectionRepository.GetLastOrder(request.DocumentId, courseType);
            var attendanceCollection = new Domain.Entities.AttendanceCollection
            {
                DocumentID = request.DocumentId,
                HeldOn = DateTime.Parse(request.ActivityDateTime),
                CourseType = courseType,
                Order = maxOrder + 1
            };

            unitOfWork.AttendanceCollectionRepository.AddAsync(attendanceCollection);

            if (!await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            // get the current document
            var document = await unitOfWork.DocumentRepository.GetDocumentByIdAsync(request.DocumentId)
                ?? throw new NotFoundException("Document", request.DocumentId);
            // get all the students according to the document data
            var students = await unitOfWork.UserSpecializationRepository.GetUserSpecializationsByExpression(
                us => us.User!.EnrollmentYear == document.EnrollmentYear &&
                    us.SpecializationID == document.Course!.UserSpecialization!.SpecializationID &&
                    us.User!.Role == Role.Student &&
                    !us.User.IsDeleted);

            // add each student as not present and with 0 bonus points
            foreach (var student in students)
            {
                unitOfWork.AttendanceRepository.AddAsync(new Domain.Entities.Attendance
                {
                    UpdatedOn = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    AttendanceCollectionID = attendanceCollection.AttendanceCollectionID,
                    BonusPoints = 0,
                    IsPresent = false,
                    UserID = student.UserID,
                });
            }

            if (!await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }



            return attendanceCollection.AttendanceCollectionID;
        }
    }
}
