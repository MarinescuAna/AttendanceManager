using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetStudentsForCourses
{
    public sealed class GetStudentsForCoursesQueryHandler : BaseFeature, IRequestHandler<GetStudentsForCoursesQuery, List<StudentDto>>
    {
        public GetStudentsForCoursesQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<List<StudentDto>> Handle(GetStudentsForCoursesQuery request, CancellationToken cancellationToken)
            =>mapper.Map<List<StudentDto>>((await unitOfWork.UserRepository.ListAllAsync(NavigationPropertiesSetting.OnlyCollectionNavigationProps))
                .Where(u => u.EnrollmentYear == int.Parse(request.EnrollmentYear) && u.UserSpecializations!.Any(s => s.SpecializationID.ToString() == request.SpecializationId) && u.Role == Role.Student)
                .ToList());
    }
}
