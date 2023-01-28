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

            return attendanceCollection.AttendanceCollectionID;
        }
    }
}
