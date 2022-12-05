using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Shared;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AttendanceManager.Application.Features.User.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : UserFeatureBase, IRequestHandler<UpdateUserCommand>
    {
        public UpdateUserCommandHandler(IUserRepository userRepo, IMapper map) : base(userRepo, map)
        {
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // Look for the user to be sure that he exists or throw exeception if he dosen't exists
            if (await userRepository.GetAsync(u=>u.Email == request.User.Email,false) == null)
            {
                throw new NotFoundException("User", request.User.Email);
            }

            // Update the user information or thow exception if something happened
            if (!await userRepository.UpdateAsync(request.User))
            {
                throw new SomethingWentWrongException(Constants.SomethingWentWrongMessage);
            }

            return Unit.Value;

        }
    }
}
