using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
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
            => mapper.Map<List<StudentDto>>(await unitOfWork.UserSpecializationRepository.GetUserSpecializationsByExpression(
                us => us.User!.EnrollmentYear == request.EnrollmentYear && 
                    us.SpecializationID == request.SpecializationId &&
                    us.User!.Role == Role.Student &&
                    !us.User.IsDeleted));
    }
}
