using AttendanceManager.Application.Contracts.UnitOfWork;
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
          => mapper.Map<List<StudentsAttendanceDTO>>((await unitOfWork.AttendanceRepository.ListAllAsync())
              .Where(df => df.AttendanceCollectionID == request.AttendanceCollectionId));
    }
}
