using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Queries.GetStudentsForCourses
{
    /// <summary>
    /// Get the list with all the students that have the given enrollment year and specialization id
    /// </summary>
    public sealed class GetStudentsForCoursesQuery : IRequest<List<StudentVm>>
    {
        public required int EnrollmentYear { get; init; }
        public required int SpecializationId { get; init; }
    }
    public sealed class GetStudentsForCoursesQueryHandler : IRequestHandler<GetStudentsForCoursesQuery, List<StudentVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetStudentsForCoursesQueryHandler(IUnitOfWork unit, IMapper mapper)
        {
            _unitOfWork = unit;
            _mapper = mapper;
        }

        public async Task<List<StudentVm>> Handle(GetStudentsForCoursesQuery request, CancellationToken cancellationToken)
            => _mapper.Map<List<StudentVm>>(await _unitOfWork.UserSpecializationRepository.GetUserSpecializationsByExpression(
                us => us.User!.Year == request.EnrollmentYear &&
                    us.SpecializationID == request.SpecializationId &&
                    us.User!.Role == Role.Student));
    }
}
