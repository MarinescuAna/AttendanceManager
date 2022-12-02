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
            if (await _mediator.Send(new GetUserByEmailQuery()
            {
                Email = request.User.Email
            }) == null)
            {
                throw new NotFoundException("User", request.User.Email);
            }

            if(!await userRepository.UpdateAsync(request.User))
            {
                throw new SomethingWentWrongException(Constants.SomethingWentWrongMessage);
            }

            return Unit.Value;

        }
    }
}
