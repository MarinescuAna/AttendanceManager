using AttendanceManager.Application.Contracts.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Course.Queries.GetCoursesQuery
{
    public sealed class GetCoursesQueryHandler : BaseFeature, IRequestHandler<GetCoursesQuery, List<CoursesDto>>
    {
        public GetCoursesQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }
        //TODO change this!!!
        public async Task<List<CoursesDto>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
            => string.IsNullOrEmpty(request.Email)? mapper.Map<List<CoursesDto>>(
                await unitOfWork.CourseRepository.ListAllAsync(Domain.Enums.NavigationPropertiesSetting.OnlyReferenceNavigationProps)
                ):
            mapper.Map<List<CoursesDto>>(
                (await unitOfWork.CourseRepository.ListAllAsync(Domain.Enums.NavigationPropertiesSetting.OnlyReferenceNavigationProps))
                    .Where(c => c.UserID.ToString() == request.Email)
                );
    }
}
