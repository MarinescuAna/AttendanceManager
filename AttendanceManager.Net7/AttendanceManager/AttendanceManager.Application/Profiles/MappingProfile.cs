using AttendanceManager.Application.SharedDtos;
using AttendanceManager.Application.Features.User.Queries.GetUserByEmail;
using AttendanceManager.Domain.Entities;
using AutoMapper;
using AttendanceManager.Application.Features.Course.Queries.GetCoursesQuery;

namespace AttendanceManager.Application.Profiles
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Department, OrganizationDto>()
                .ForMember(d=>d.Id,act=>act.MapFrom(d=>d.DepartmentID))
                .ForMember(d=>d.Children,act=>act.MapFrom(d=>d.Specializations));
            CreateMap<Specialization, SpecializationDto>()
                .ForMember(d => d.Id, act => act.MapFrom(d => d.SpecializationID));
            CreateMap<Department, DepartmentDto>()
                .ForMember(d => d.Id, act => act.MapFrom(d => d.DepartmentID));
            CreateMap<Specialization, Features.Specialization.Commands.CreateSpecialization.SpecializationDto>()
                .ForMember(d => d.Id, act => act.MapFrom(d => d.SpecializationID))
                .ForMember(s=>s.DepartmentId, act=>act.MapFrom(s=>s.DepartmentID));
            CreateMap<User, Features.User.Queries.GetAllUsers.UserDto>();
            CreateMap<Course, CoursesDto>();
            CreateMap<Course, CoursesDto>()
                .ForMember(d => d.SpecializationName, act => act.MapFrom(d => d.Specialization!.Name))
                .ForMember(s => s.SpecializationId, act => act.MapFrom(s => s.SpecializationID));
        }
    }
}
