using AttendanceManager.Application.Contracts.Authentication;
using AttendanceManager.Application.Features.User.Commands.ConfirmUserAccount;
using AttendanceManager.Application.Features.User.Commands.UpdateRefreshToken;
using AttendanceManager.Application.Features.User.Queries.GetUserByEmail;
using AttendanceManager.Application.Models.Authentication;
using MediatR;

namespace AttendanceManager.Infrastructure.Authentication
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly IMediator _mediator;
        private readonly IJsonWebTokenService _jwtService;

        public AuthenticationService(IMediator mediator, IJsonWebTokenService jsonWebTokenService)
        {
            _jwtService = jsonWebTokenService;
            _mediator = mediator;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            // Return the current user according to the email
            var result = await _mediator.Send(new GetUserByEmailQuery() { Email = request.Email })
                ?? throw new Exception($"User with {request.Email} not found.");

            // Check the passwords
            if (result.Password != request.Password)
            {
                throw new Exception($"Credentials for '{request.Email} aren't valid.");
            }

            // If the user access the account for the first time
            if (!result.AccountConfirmed)
            {
                // Mark the account as confirmed and update the data 
                result.AccountConfirmed = true;
                await _mediator.Send(new ConfirmUserAccountCommand() { Email = request.Email });
            }

            // Get token
            var accessToken = _jwtService.GenerateAccessToken(result.Email, result.FullName, result.Role, result.Code);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Update refresh token
            await _mediator.Send(new UpdateRefreshTokenCommand
            {
                Email = request.Email,
                ExpRefreshToken = refreshToken.Expiration,
                RefreshToken = refreshToken.Token
            });

            return new()
            {
                ExpirationDateAccessToken = accessToken.Expiration,
                AccessToken = accessToken.Token,
                RefreshToken = refreshToken.Token,
                ExpirationDateRefreshToken = refreshToken.Expiration
            };

        }
    }
}
