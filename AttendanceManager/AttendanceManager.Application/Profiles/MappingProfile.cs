using AttendanceManager.Application.SharedDtos;
using AttendanceManager.Application.Features.Department.Queries.GetDepartments;
using AttendanceManager.Application.Features.User.Queries.GetUserByEmail;
using AttendanceManager.Domain.Entities;
using AutoMapper;

namespace AttendanceManager.Application.Profiles
{
    public class MappingProfile : Profile
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
        }
    }
}
