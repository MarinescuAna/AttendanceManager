using AttendanceManager.Application.Contracts.Authentication;
using AttendanceManager.Application.Models.Authentication;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System;
using System.Threading.Tasks;
using AttendanceManager.Application.Features.User.Queries.GetUserByEmail;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using MediatR;
using Microsoft.Extensions.Options;

namespace AttendanceManager.Infrastructure.Authentication
{
    public class AuthenticationService : IAuthenticationService
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
            //return the current user according to the email
            var result = await _mediator.Send(new GetUserByEmailQuery() { Email = request.Email});
            
            //check if the user was found
            if (result == null)
            {
                throw new Exception($"User with {request.Email} not found.");
            }

            //check the passwords
            if (result.Password != request.Password)
            {
                throw new Exception($"Credentials for '{request.Email} aren't valid.");
            }

            var jwtSecurityToken = GenerateToken(result);

            AuthenticationResponse response = new AuthenticationResponse
            {
                Token = jwtSecurityToken
                //TODO add refresh token
            };

            return response;
        }
        private string GenerateToken(UserVm user)
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
