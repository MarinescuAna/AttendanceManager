using AttendanceManager.Application.Features.Department.Queries.GetDepartments;
using AttendanceManager.Domain.Entities;
using AutoMapper;
namespace AttendanceManager.Application.Profiles
{
    public sealed class DepartmentMappingProfiler : Profile
    {
        public DepartmentMappingProfiler()
        {
            CreateMap<Department, DepartmentDto>()
                .ForMember(d => d.Id, act => act.MapFrom(d => d.DepartmentID));
        }

    }
}
