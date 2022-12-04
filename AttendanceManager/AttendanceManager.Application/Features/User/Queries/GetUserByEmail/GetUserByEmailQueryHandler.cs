using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Application.Exceptions;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AttendanceManager.Application.Features.User.Queries.GetUserByEmail
{
    public class GetUserByEmailQueryHandler :UserFeatureBase, IRequestHandler<GetUserByEmailQuery, UserDto>
    {
        public GetUserByEmailQueryHandler(IMapper mapper, IUserRepository userRepository):base(userRepository,mapper)
        {
        }

        public async Task<UserDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            // Get the user by email
            var user = await userRepository.GetAsync(u => u.Email == request.Email);

            // Throw exception if the user dosen't exists
            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.Email);
            }

            return mapper.Map<UserDto>(user);

        }
    }
}
