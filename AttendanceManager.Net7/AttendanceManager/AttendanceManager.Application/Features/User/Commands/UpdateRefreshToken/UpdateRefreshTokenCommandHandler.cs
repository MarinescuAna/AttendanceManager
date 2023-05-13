using AttendanceManager.Application.Contracts.Persistance.UnitOfWork;
using AttendanceManager.Application.Exceptions;
using AttendanceManager.Core.Shared;
using AutoMapper;
using MediatR;

namespace AttendanceManager.Application.Features.User.Commands.UpdateRefreshToken
{
    public sealed class UpdateRefreshTokenCommandHandler : BaseFeature, IRequestHandler<UpdateRefreshTokenCommand>
    {
        public UpdateRefreshTokenCommandHandler(IUnitOfWork unit, IMapper mapper) : base(unit, mapper)
        {
        }

        public async Task<Unit> Handle(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // Look for the user to be sure that he exists or throw exeception if he dosen't exists
            var user = await unitOfWork.UserRepository.GetAsync(u => (u.Email == request.Email) && !u.IsDeleted)
            ?? throw new NotFoundException("User", request.Email);

            user.RefreshToken = request.RefreshToken;
            user.ExpRefreshToken = request.ExpRefreshToken;

            // Update the user information or thow exception if something happened
            unitOfWork.UserRepository.Update(user);

            if (!await unitOfWork.CommitAsync())
            {
                throw new SomethingWentWrongException(ErrorMessages.SomethingWentWrongGenericMessage);
            }

            return Unit.Value;
        }
    }
}
