using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Core.Shared;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Course.Commands.CreateCourse
{
    public sealed class CreateCourseCommandHandler : BaseFeature, IRequestHandler<CreateCourseCommand, int>
    {
        public CreateCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<int> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            // Check if the name is unique
            if (await unitOfWork.CourseRepository.GetAsync(c => c.UserSpecializationID == request.SpecializationId && c.Name == request.Name && !c.IsDeleted) != null)
            {
                throw new AlreadyExistsException("Course", request.Name);
            }
            var userSpecialization = await unitOfWork.UserSpecializationRepository.GetAsync(u => u.UserID == request.Email && u.SpecializationID == request.SpecializationId)
                ?? throw new NotFoundException("UserSpecialization", string.Empty);

            var newCourse = new Domain.Entities.Course
            {
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                Name = request.Name,
                IsDeleted = false,
                UserSpecializationID = userSpecialization.UserSpecializationID
            };

            unitOfWork.CourseRepository.AddAsync(newCourse);

            if (!await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(Constants.SomethingWentWrongMessage);
            }

            return newCourse.CourseID;
        }
    }
}
