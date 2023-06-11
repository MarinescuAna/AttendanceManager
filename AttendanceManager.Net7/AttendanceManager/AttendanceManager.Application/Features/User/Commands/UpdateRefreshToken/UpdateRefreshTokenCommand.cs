using AttendanceManager.Application.Contracts.Infrastructure.Mail;
using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Domain.Common;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Commands.UpdateRefreshToken
{
    public sealed class UpdateRefreshTokenCommand : IRequest
    {
        public required string Email { get; init; }
        public required string RefreshToken { get; init; }
        public required DateTime ExpRefreshToken { get; init; }

    }
    public sealed class UpdateRefreshTokenCommandHandler : IRequestHandler<UpdateRefreshTokenCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateRefreshTokenCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // Look for the user to be sure that he exists or throw exeception if he dosen't exists
            var user = await _unitOfWork.UserRepository.GetAsync(u => (u.Email == request.Email) && !u.IsDeleted)
            ?? throw new NotFoundException("User", request.Email);

            user.RefreshToken = request.RefreshToken;
            user.ExpRefreshToken = request.ExpRefreshToken;

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
