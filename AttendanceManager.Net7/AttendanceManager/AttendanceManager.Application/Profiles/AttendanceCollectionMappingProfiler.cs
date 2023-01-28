
using AttendanceManager.Application.Features.AttendanceCollection.Queries.GetAttendanceCollectionByDocumentId;
using AttendanceManager.Domain.Entities;
using AutoMapper;

namespace AttendanceManager.Application.Profiles
{
    public sealed class AttendanceCollectionMappingProfiler : Profile
    {
        public AttendanceCollectionMappingProfiler()
        {
            CreateMap<AttendanceCollection, AttendanceCollectionDto>()
                .ForMember(a => a.ActivityTime, act => act.MapFrom(ac => ac.HeldOn));
        }
    }
}
