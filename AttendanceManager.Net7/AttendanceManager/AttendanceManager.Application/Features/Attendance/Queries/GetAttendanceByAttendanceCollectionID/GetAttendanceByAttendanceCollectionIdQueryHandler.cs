using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Queries.GetAttendanceByAttendanceCollectionID
{
    public sealed class GetAttendanceByAttendanceCollectionIdQueryHandler : BaseFeature, IRequestHandler<GetAttendanceByAttendanceCollectionIdQuery, List<StudentsAttendanceDTO>>
    {
        public GetAttendanceByAttendanceCollectionIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<List<StudentsAttendanceDTO>> Handle(GetAttendanceByAttendanceCollectionIdQuery request, CancellationToken cancellationToken)
        {
            var attendances = (await unitOfWork.AttendanceCollectionRepository.GetAttendanceCollectionByIdAsync(request.AttendanceCollectionId)).Attendances!;

            if(request.Role == Domain.Enums.Role.Student && attendances.Count>0)
            {
                var users = (await unitOfWork.UserRepository.ListAllAsync()).Where(u => attendances.Any(a => a.UserID.Equals(u.Email)))
                        .ToDictionary(k => k.Email, v => v.Code);
                foreach (var attendance in attendances)
                {
                    attendance.UserID = users[attendance.UserID];
                }
            }

            return mapper.Map<List<StudentsAttendanceDTO>>(attendances);
        } 
    }
}
