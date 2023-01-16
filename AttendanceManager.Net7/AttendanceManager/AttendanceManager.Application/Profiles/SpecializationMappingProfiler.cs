using AttendanceManager.Application.Features.Specialization.Queries.GetSpecializations;
using AttendanceManager.Domain.Entities;
using AutoMapper;

namespace AttendanceManager.Application.Profiles
{
    public sealed class SpecializationMappingProfiler : Profile
    {
        public SpecializationMappingProfiler()
        {
            CreateMap<Specialization, SpecializationDto>()
                .ForMember(d => d.Id, act => act.MapFrom(d => d.SpecializationID));
        }
    }
}
