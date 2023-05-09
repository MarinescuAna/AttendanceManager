using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Course.Queries.GetCoursesQuery
{
    public sealed class GetCoursesQueryHandler : BaseFeature, IRequestHandler<GetCoursesQuery, List<CoursesDto>>
    {
        public GetCoursesQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }
        public async Task<List<CoursesDto>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
            => mapper.Map<List<CoursesDto>>(await unitOfWork.CourseRepository.GetTeacherCoursesByEmailAsync(request.Email));
    }
}
