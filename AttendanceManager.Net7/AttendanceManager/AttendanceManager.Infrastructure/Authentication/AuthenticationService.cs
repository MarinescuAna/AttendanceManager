using AttendanceManager.Application.Features.User.Commands.ConfirmUserAccount;
using AttendanceManager.Application.Features.User.Queries.GetUserByEmail;
using AttendanceManager.Application.Models.Authentication;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AttendanceManager.Infrastructure.Authentication
{
    public sealed class AuthenticationService : Application.Contracts.Authentication.IAuthenticationService
    {
        private readonly IMediator _mediator;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(IOptions<JwtSettings> jwtSettings, IMediator mediator)
        {
            _jwtSettings = jwtSettings.Value;
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
            var jwtSecurityToken = GenerateToken(result);

            return new AuthenticationResponse
            {
                Token = jwtSecurityToken
                //TODO add refresh token
            };
        }
        private string GenerateToken(UserDto user)
        {
            // define a bunch of clams
            var claims = new[]
            {
                new Claim("email", user.Email),
                new Claim("name", user.FullName),
                new Claim("role", user.Role),
                new Claim("code", string.IsNullOrEmpty(user.Code)? string.Empty:user.Code )
            };

            // grab the security key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            //define a credential object base on the security key definded above
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            //deifine the token object
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        }
    }
}
