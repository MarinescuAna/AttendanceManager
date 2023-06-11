using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using MediatR;

namespace AttendanceManager.Application.Features.User.Commands.ConfirmUserAccount
{
    public sealed class ConfirmUserAccountCommand : IRequest
    {
        public required string Email { get; init; }
    }
    public sealed class ConfirmUserAccountCommandHandler : IRequestHandler<ConfirmUserAccountCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ConfirmUserAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ConfirmUserAccountCommand request, CancellationToken cancellationToken)
        {
            // Look for the user to be sure that he exists or throw exeception if he dosen't exists
            var user = await _unitOfWork.UserRepository.GetAsync(u => u.Email == request.Email && !u.IsDeleted)
                ?? throw new NotFoundException("User", request.Email);

            user.AccountConfirmed = true;

            // Update the user information or thow exception if something happened
            _unitOfWork.UserRepository.Update(user);

            if (!await _unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            return Unit.Value;

        }
    }
}
