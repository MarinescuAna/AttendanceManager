using AttendanceManager.Application.Features.Attendance.Queries.GetAttendanceByAttendanceCollectionID;
using AttendanceManager.Domain.Entities;
using AttendanceManager.Application.Shared;
using AutoMapper;

namespace AttendanceManager.Application.Profiles
{
    public sealed class AttendanceMappingProfiler : Profile
    {
        public AttendanceMappingProfiler()
        {
            CreateMap<Attendance, StudentsAttendanceDTO>()
                .ForMember(a => a.UpdatedOn, act => act.MapFrom(ac => ac.UpdatedOn.ToString(Constants.DateFormat)));
        }

    }
}
