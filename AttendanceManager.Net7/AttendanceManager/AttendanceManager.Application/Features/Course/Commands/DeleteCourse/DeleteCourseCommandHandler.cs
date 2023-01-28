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
            if (!await unitOfWork.CourseRepository.SoftOrHardDelete(request.Id))
            {
                throw new NotFoundException("Course", request.Id);
            }

            return await unitOfWork.CommitAsync();
        }
    }
}
