using AttendanceManager.Domain.Entities;
using AutoMapper;

namespace AttendanceManager.Application.Profiles
{
    public sealed class MappingProfiler : Profile
    {
        public MappingProfiler()
        {
            // Used for involvement codes when we create an involvement code and return something that is not void or null
            CreateMap<InvolvementCode, Features.InvolvementCode.Commands.CreateInvolvementCode.InvolvementCodeDto>();

            // Used for bagdes when se take the doc info
            CreateMap<Badge, Dtos.BadgeDto>()
                .ForMember(d=>d.Type, act => act.MapFrom(b=>b.BadgeType));
        }
    }
}
