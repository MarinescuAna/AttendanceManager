using AttendanceManager.Application.Contracts.Persistance;
using AutoMapper;

namespace AttendanceManager.Application.Features.User
{
    public abstract class UserFeatureBase
    {
        protected readonly IMapper mapper;
        protected readonly IUserRepository userRepository;
        public UserFeatureBase(IUserRepository userRepo, IMapper map)
        {
            userRepository = userRepo;
            mapper = map;
        }
    }
}
