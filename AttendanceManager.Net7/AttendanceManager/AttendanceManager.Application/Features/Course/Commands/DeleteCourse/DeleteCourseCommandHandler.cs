using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Course.Commands.DeleteCourse
{
    public sealed class DeleteCourseCommandHandler : BaseFeature, IRequestHandler<DeleteCourseCommand, bool>
    {
        public DeleteCourseCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await unitOfWork.CourseRepository.GetAsync(c => c.CourseID.ToString() == request.Id && !c.IsDeleted);

            if (course == null)
            {
                throw new NotFoundException("Course", request.Id);
            }

            course.IsDeleted = true;
            unitOfWork.CourseRepository.Update(course);

            return await unitOfWork.CommitAsync();
        }
    }
}
