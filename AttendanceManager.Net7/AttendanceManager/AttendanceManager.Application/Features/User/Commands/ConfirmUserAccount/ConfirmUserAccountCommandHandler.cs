using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Core.Shared;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Commands.ConfirmUserAccount
{
    public sealed class ConfirmUserAccountCommandHandler : BaseFeature, IRequestHandler<ConfirmUserAccountCommand>
    {
        public ConfirmUserAccountCommandHandler(IUnitOfWork unitOfWork, IMapper map) : base(unitOfWork, map)
        {
        }

        public async Task<Unit> Handle(ConfirmUserAccountCommand request, CancellationToken cancellationToken)
        {
            // Look for the user to be sure that he exists or throw exeception if he dosen't exists
            var user = await unitOfWork.UserRepository.GetAsync(u => u.Email == request.Email && !u.IsDeleted) 
                ?? throw new NotFoundException("User", request.Email);

            user.AccountConfirmed = true;

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
