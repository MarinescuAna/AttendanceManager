using AttendanceManager.Application.Features.User.Queries.Dtos;
using AttendanceManager.Domain.Entities;
using AutoMapper;

namespace AttendanceManager.Application.Profiles
{
    public sealed class UserMappingProfiler : Profile
    {
        public UserMappingProfiler()
        {
            CreateMap<User, UserByEmailDto>();
            CreateMap<User, UserByRefreshTokenDto>();
        }
    }
}
