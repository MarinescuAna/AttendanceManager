using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Shared;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.AttendanceCollection.Commands.CreateAttendanceCollection
{
    public sealed class CreateAttendanceCollectionCommandHandler :BaseFeature, IRequestHandler<CreateAttendanceCollectionCommand, int>
    {
        public CreateAttendanceCollectionCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<int> Handle(CreateAttendanceCollectionCommand request, CancellationToken cancellationToken)
        {
            var attendanceCollection = new Domain.Entities.AttendanceCollection
            {
                DocumentID = request.DocumentId,
                HeldOn = DateTime.Parse(request.ActivityDateTime),
                CourseType = (CourseType)Enum.Parse(typeof(CourseType), request.CourseType)
            };

            unitOfWork.AttendanceCollectionRepository.AddAsync(attendanceCollection);

            if (!await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(Constants.SomethingWentWrongMessage);
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
            foreach(var student in students)
            {
                unitOfWork.AttendanceRepository.AddAsync( new Domain.Entities.Attendance { 
                    UpdatedOn = DateTime.UtcNow,
                    CreatedOn= DateTime.UtcNow, 
                    AttendanceCollectionID = attendanceCollection.AttendanceCollectionID,
                    BonusPoints = 0,
                    IsPresent = false,
                    UserID = student.UserID,
                });
            }

            if (!await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(Constants.SomethingWentWrongMessage);
            }

            return attendanceCollection.AttendanceCollectionID;
        }
    }
}
