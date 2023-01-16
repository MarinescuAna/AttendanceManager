using AttendanceManager.Application.Features.Course.Queries.GetCoursesQuery;
using AttendanceManager.Domain.Entities;
using AutoMapper;

namespace AttendanceManager.Application.Profiles
{
    public sealed class CourseMappingProfiler : Profile
    {
        public CourseMappingProfiler()
        {
            CreateMap<Course, CoursesDto>()
                .ForMember(d => d.SpecializationName, act => act.MapFrom(d => d.Specialization!.Name))
                .ForMember(s => s.SpecializationId, act => act.MapFrom(s => s.SpecializationID));
        }
    }
}
