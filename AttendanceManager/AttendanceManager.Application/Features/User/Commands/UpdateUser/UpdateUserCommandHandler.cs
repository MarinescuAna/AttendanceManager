using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Features.User.Queries.GetUserByEmail;
using AttendanceManager.Application.Shared;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AttendanceManager.Application.Features.User.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : UserFeatureBase, IRequestHandler<UpdateUserCommand>
    {
        private readonly IMediator _mediator;
        public UpdateUserCommandHandler(IUserRepository userRepo, IMapper map, IMediator mediator) : base(userRepo, map)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // Look for the user to be sure that he exists or throw exeception if he dosen't exists
            if (await _mediator.Send(new GetUserByEmailQuery() { Email = request.User.Email }) == null)
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
