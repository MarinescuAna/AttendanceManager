using AttendanceManager.Application.Features.User.Queries.GetStudentsForCourses;
using AttendanceManager.Domain.Entities;
using AutoMapper;

namespace AttendanceManager.Application.Profiles
{
    public sealed class SpecializationMappingProfiler : Profile
    {
        public SpecializationMappingProfiler()
        {
            CreateMap<UserSpecialization, Features.User.Queries.GetUserInformationByEmail.SpecializationDto>()
                .ForMember(d => d.Id, act => act.MapFrom(d => d.SpecializationID))
                .ForMember(d => d.Name, act => act.MapFrom(d => d.Specialization!.Name));

            CreateMap<UserSpecialization, StudentDto>()
                .ForMember(d => d.Email, act => act.MapFrom(d => d.UserID))
                .ForMember(d => d.Fullname, act => act.MapFrom(d => d.User!.FullName));
        }
    }
}
