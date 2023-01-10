using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Course.Commands.UpdateCourseName
{
    public sealed class UpdateCourseNameCommandHandler : BaseFeature, IRequestHandler<UpdateCourseNameCommand, bool>
    {
        public UpdateCourseNameCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<bool> Handle(UpdateCourseNameCommand request, CancellationToken cancellationToken)
        {
            var course = await unitOfWork.CourseRepository.GetAsync(c => c.SpecializationID.ToString() == request.CourseId && !c.IsDeleted);

            if (course == null)
            {
                throw new NotFoundException("Course", request.CourseId);
            }

            course.Name = request.Name;
            unitOfWork.CourseRepository.Update(course);

            return await unitOfWork.CommitAsync();
        }
    }
}
