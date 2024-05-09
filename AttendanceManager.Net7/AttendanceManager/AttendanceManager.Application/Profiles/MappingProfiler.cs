using AttendanceManager.Domain.Common;
using AttendanceManager.Domain.Entities;
using AutoMapper;

namespace AttendanceManager.Application.Profiles
{
    public sealed class MappingProfiler : Profile
    {
        public MappingProfiler()
        {
            //Used to get departments
            CreateMap<Department, Features.Department.Queries.GetDepartments.DepartmentVm>()
                .ForMember(d => d.Id, act => act.MapFrom(d => d.DepartmentID))
                .ForMember(d => d.UpdatedOn, act => act.MapFrom(d => d.UpdatedOn.ToString(Constants.ShortDateFormat)))
                .ForMember(d => d.LinkedSpecializations, act => act.MapFrom(d=>d.Specializations!.Count()));

            //Used to get specializations with the departmentId
            CreateMap<Specialization, Features.Specialization.Queries.GetSpecializations.SpecializationVm>()
             .ForMember(d => d.Id, act => act.MapFrom(d => d.SpecializationID))
             .ForMember(d => d.UpdatedOn, act => act.MapFrom(d => d.UpdatedOn.ToString(Constants.ShortDateFormat)))
             .ForMember(d => d.UsersLinked, act => act.MapFrom(d => d.UserSpecializations!.Count()));

            // Used for involvement codes when we create an involvement code and return something that is not void or null
            CreateMap<InvolvementCode, Features.InvolvementCode.Commands.CreateInvolvementCode.InvolvementCodeVm>();

            // Used for bagdes when we try to retrive all the badges to display them
            CreateMap<Reward, Features.Reward.Queries.GetAllRewardsByUserIdReportId.RewardVm>()
                .ForMember(d => d.ImagePath, act => act.MapFrom(b => b.Badge!.ImagePath))
                .ForMember(d => d.Title, act => act.MapFrom(b => b.Badge!.Title))
                .ForMember(d => d.BadgeType, act => act.MapFrom(b => b.Badge!.BadgeType))
                .ForMember(d => d.MaxNumber, act => act.MapFrom(b => b.Badge!.MaxNumber))
                .ForMember(d => d.RewardId, act => act.MapFrom(r => r.RewardID))
                .ForMember(d => d.IsCustom, act => act.MapFrom(r => r.Badge!.ReportID != null))
                .ForMember(d => d.Description, act => act.MapFrom(r => r.Badge!.Description))
                .ForMember(d => d.IsActive, act => act.MapFrom(_ => true));

            //Used for getting the notifications
            CreateMap<Notification, Features.Notification.Queries.GetNotificationsByUserId.NotificationVm>();

            //Used when we get info about a report
            CreateMap<Collection, Features.Report.Queries.GetReportById.CollectionDto>()
                .ForMember(a => a.CollectionId, act => act.MapFrom(ac => ac.CollectionID))
                .ForMember(a=>a.CourseType, act => act.MapFrom(ac=>ac.ActivityType))
                .ForMember(a => a.ActivityTime, act => act.MapFrom(ac => ac.HeldOn.ToString(Constants.ShortDateFormat)));
            CreateMap<Member, Features.Report.Queries.GetReportById.MembersDto>()
                .ForMember(u => u.Name, act => act.MapFrom(d => d.User!.FullName))
                .ForMember(u => u.Email, act => act.MapFrom(d => d.User!.Email));

            //Used when get courses
            CreateMap<Course, Features.Course.Queries.GetCoursesQuery.CoursesVm>()
                .ForMember(d => d.SpecializationName, act => act.MapFrom(d => d.UserSpecialization!.Specialization!.Name))
                .ForMember(d => d.SpecializationId, act => act.MapFrom(d => d.UserSpecialization!.SpecializationID))
                .ForMember(d => d.ReportsLinked, act => act.MapFrom(d => d.Reports!.Count()))
                .ForMember(d=>d.UpdatedOn, act => act.MapFrom(d=>d.UpdatedOn.ToString(Constants.ShortDateFormat)));

            //used to get users by year and specialization to create a new report
            CreateMap<UserSpecialization, Features.User.Queries.GetStudentsForCourses.StudentVm>()
                .ForMember(d => d.Email, act => act.MapFrom(d => d.UserID))
                .ForMember(d => d.Fullname, act => act.MapFrom(d => d.User!.FullName));

            //used to get the information about the current user
            CreateMap<UserSpecialization, Features.User.Queries.GetUserInformationByEmail.SpecializationDto>()
                .ForMember(d => d.Id, act => act.MapFrom(d => d.SpecializationID))
                .ForMember(d => d.Name, act => act.MapFrom(d => d.Specialization!.Name));

            //used to get the user by refresh token
            CreateMap<User, Features.User.Queries.GetUserByRefreshToken.UserByRefreshTokenVm>();

            //used for login
            CreateMap<User, Features.User.Queries.GetUserByEmail.UserByEmailVm>();
        }
    }
}
