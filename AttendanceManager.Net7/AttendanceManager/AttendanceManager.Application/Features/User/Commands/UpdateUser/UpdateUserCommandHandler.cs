using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Application.Shared;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Commands.UpdateUser
{
    public sealed class UpdateUserCommandHandler : BaseFeature, IRequestHandler<UpdateUserCommand>
    {
        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper map) : base(unitOfWork, map)
        {
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // Look for the user to be sure that he exists or throw exeception if he dosen't exists
            var user = await unitOfWork.UserRepository.GetAsync(u => u.Email == request.Email) ?? throw new NotFoundException("User", request.Email);

            if(request.Confirmed!= null)
            {
                user.AccountConfirmed = (bool)request.Confirmed;
            }

            // Update the user information or thow exception if something happened
            unitOfWork.UserRepository.Update(user);
            if (!await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(Constants.SomethingWentWrongMessage);
            }

            return Unit.Value;

        }
    }
}
