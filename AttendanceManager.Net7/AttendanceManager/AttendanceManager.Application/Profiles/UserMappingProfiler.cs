using AttendanceManager.Domain.Entities;
using AutoMapper;

namespace AttendanceManager.Application.Profiles
{
    public sealed class UserMappingProfiler : Profile
    {
        public UserMappingProfiler()
        {
            CreateMap<User, Features.User.Queries.GetUserByEmail.UserDto>();
        }
    }
}
