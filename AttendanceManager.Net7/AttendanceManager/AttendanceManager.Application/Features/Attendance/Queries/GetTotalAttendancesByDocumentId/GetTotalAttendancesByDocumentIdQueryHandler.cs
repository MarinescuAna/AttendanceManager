using AttendanceManager.Application.Contracts.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.Attendance.Queries.GetTotalAttendancesByDocumentId
{
    public sealed class GetTotalAttendancesByDocumentIdQueryHandler : BaseFeature, IRequestHandler<GetTotalAttendancesByDocumentIdQuery, List<TotalAttendanceDTO>>
    {
        public GetTotalAttendancesByDocumentIdQueryHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<List<TotalAttendanceDTO>> Handle(GetTotalAttendancesByDocumentIdQuery request, CancellationToken cancellationToken)
        {
            var totalAttendances = new List<TotalAttendanceDTO>();
            var collectionAttendances = (await unitOfWork.AttendanceCollectionRepository.GetAttendanceCollectionsByDocumentIdAsync(request.DocumentId))
                .ToDictionary(ca=>ca.AttendanceCollectionID,ca=>ca.CourseType);
            var students = await unitOfWork.DocumentMemberRepository.GetStudentsByDocumentIdAsync(request.DocumentId);

            foreach(var student in students) {
                var attendances = student.Attendances!.Where(a => collectionAttendances.ContainsKey(a.AttendanceCollectionID)).ToList();

                totalAttendances.Add(new()
                {
                    UserID = student.Email,
                    UserName = student.FullName,
                    Code = student.Code,
                    BonusPoints = attendances.Sum(a => a.BonusPoints),
                    CourseAttendances = attendances.Where(a=>a.IsPresent && collectionAttendances[a.AttendanceCollectionID]==Domain.Enums.CourseType.Lesson).Count(),
                    LaboratoryAttendances = attendances.Where(a=>a.IsPresent && collectionAttendances[a.AttendanceCollectionID] == Domain.Enums.CourseType.Laboratory).Count(),
                    SeminaryAttendances = attendances.Where(a=>a.IsPresent && collectionAttendances[a.AttendanceCollectionID] == Domain.Enums.CourseType.Seminary).Count(),
                });
            }

            return totalAttendances;
        }
    }
}
