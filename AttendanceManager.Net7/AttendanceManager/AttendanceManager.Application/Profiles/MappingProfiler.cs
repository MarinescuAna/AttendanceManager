using AttendanceManager.Domain.Entities;
using AutoMapper;

namespace AttendanceManager.Application.Profiles
{
    public sealed class MappingProfiler : Profile
    {
        public MappingProfiler()
        {
            CreateMap<InvolvementCode, Features.InvolvementCode.Commands.CreateInvolvementCode.InvolvementCodeDto>();
        }
    }
}
