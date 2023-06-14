using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.Course.Commands.CreateCourse
{
    public sealed class CreateCourseCommand : IRequest<int>
    {
        public required string Name { get; init; }
        public required int SpecializationId { get; init; }
        public string? Email { get; set; }
    }

    public sealed class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCourseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            // Check if the name is unique
            if (await _unitOfWork.CourseRepository.GetAsync(c => c.UserSpecializationID == request.SpecializationId && c.Name == request.Name) != null)
            {
                throw new AlreadyExistsException("Course", request.Name);
            }
            var userSpecialization = await _unitOfWork.UserSpecializationRepository.GetAsync(u => u.UserID == request.Email && u.SpecializationID == request.SpecializationId)
                ?? throw new NotFoundException("UserSpecialization", string.Empty);

            var newCourse = new Domain.Entities.Course
            {
                UpdatedOn = DateTime.Now,
                Name = request.Name,
                UserSpecializationID = userSpecialization.UserSpecializationID
            };

            _unitOfWork.CourseRepository.AddAsync(newCourse);

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            return newCourse.CourseID;
        }
    }
}
