using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Course.Queries.GetCoursesQuery
{
    /// <summary>
    /// Get all the courses own by the current user, which is only the teachers, not admin or students
    /// </summary>
    public sealed class GetCoursesQuery : IRequest<List<CoursesVm>>
    {
        public required string Email { get; init; }
    }
    public sealed class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, List<CoursesVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetCoursesQueryHandler(IUnitOfWork unit, IMapper mapper)
        {
            _unitOfWork = unit;
            _mapper = mapper;
        }
        public async Task<List<CoursesVm>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
            => _mapper.Map<List<CoursesVm>>(await _unitOfWork.CourseRepository.GetTeacherCoursesByEmailAsync(request.Email));
    }
}
