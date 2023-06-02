using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Entities;
using AutoMapper;

namespace AttendanceManager.Application.Profiles
{
    public sealed class MappingProfiler : Profile
    {
        public MappingProfiler()
        {
            // Used for involvement codes when we create an involvement code and return something that is not void or null
            CreateMap<InvolvementCode, Features.InvolvementCode.Commands.CreateInvolvementCode.InvolvementCodeVm>();

            // Used for bagdes when we try to retrive all the badges to display them
            CreateMap<Reward, Features.Reward.Queries.GetAllRewardsByUserIdReportId.RewardVm>()
                .ForMember(d => d.ImagePath, act => act.MapFrom(b => b.Badge!.ImagePath))
                .ForMember(d => d.Title, act => act.MapFrom(b => b.Badge!.Title))
                .ForMember(d => d.RewardId, act => act.MapFrom(r => r.RewardID))
                .ForMember(d => d.Description, act => act.MapFrom(r => r.Badge!.Description))
                .ForMember(d => d.IsActive, act => act.MapFrom(_ => true));

            //Used for getting the notifications
            CreateMap<Notification, Features.Notification.Queries.GetNotificationsByUserId.NotificationVm>();

            //Used when we get info about a report
            CreateMap<AttendanceCollection, Features.Document.Queries.GetReportById.CollectionDto>()
                .ForMember(a => a.CollectionId, act => act.MapFrom(ac => ac.AttendanceCollectionID))
                .ForMember(a => a.ActivityTime, act => act.MapFrom(ac => ac.HeldOn.ToString(Constants.ShortDateFormat)));
            CreateMap<DocumentMember, Features.Document.Queries.GetReportById.MembersDto>()
                .ForMember(u => u.Name, act => act.MapFrom(d => d.User!.FullName))
                .ForMember(u => u.Email, act => act.MapFrom(d => d.User!.Email));
        }
    }
}
