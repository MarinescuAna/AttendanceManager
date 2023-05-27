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

            // Used for bagdes when we try to retrive all the badges to display them
            CreateMap<Reward, Features.Reward.Queries.GetAllRewardsByUserIdReportId.RewardDto>()
                .ForMember(d => d.ImagePath, act => act.MapFrom(b => b.Badge!.ImagePath))
                .ForMember(d => d.Title, act => act.MapFrom(b => b.Badge!.Title))
                .ForMember(d => d.RewardId, act => act.MapFrom(r => r.RewardID))
                .ForMember(d => d.IsActive, act => act.MapFrom(_ => true));
            CreateMap<Badge, Features.Reward.Queries.GetAllRewardsByUserIdReportId.RewardDto>()
                .ForMember(d => d.RewardId, act => act.MapFrom(r => r.BadgeID))
                .ForMember(d => d.RewardId, act => act.MapFrom(_ => -1));
        }
    }
}
