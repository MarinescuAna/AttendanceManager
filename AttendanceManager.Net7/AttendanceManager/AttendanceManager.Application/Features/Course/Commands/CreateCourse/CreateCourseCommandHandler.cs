using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Shared;
using AttendanceManager.Domain.Entities;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Course.Commands.CreateCourse
{
    public sealed class CreateCourseCommandHandler :BaseFeature, IRequestHandler<CreateCourseCommand, Guid>
    {
        public CreateCourseCommandHandler(IUnitOfWork unitOfWork,IMapper mapper):base(unitOfWork,mapper)
        {
        }

        public async Task<Guid> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            // Check if the name is unique
            var course = await unitOfWork.CourseRepository.GetAsync(c=>c.SpecializationID.ToString()==request.SpecializationId && c.Name==request.Name && c.UserID==request.Email && !c.IsDeleted);
            if(course != null)
            {
                throw new AlreadyExistsException("Course", request.Name);
            }

            var newCourse = new Domain.Entities.Course
            {
                Name = request.Name,
                CourseID = Guid.NewGuid(),
                IsDeleted= false,
                SpecializationID = Guid.Parse(request.SpecializationId),
                UserID = request.Email!
            };

            unitOfWork.CourseRepository.AddAsync(newCourse);

            if(!await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(Constants.SomethingWentWrongMessage);
            }

            return newCourse.CourseID;
        }
    }
}
