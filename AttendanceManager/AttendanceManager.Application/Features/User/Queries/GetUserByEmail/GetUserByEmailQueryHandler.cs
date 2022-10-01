using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AttendanceManager.Application.Features.User.Queries.GetUserByEmail
{
    /// <summary>
    /// This is a handler for take the user by emal.
    /// GetUserByEmailQuery = the query that will be handled
    /// UserVm = the type of the response
    /// 
    /// Note: Don't forget to add the new VMs into the MappingProfile class
    /// </summary>
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserVm>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        public GetUserByEmailQueryHandler(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepo = userRepository;
        }

        public async Task<UserVm> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetAsync(u => u.Email == request.Email);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.Email);
            }

            return _mapper.Map<UserVm>(user);

        }
    }
}
