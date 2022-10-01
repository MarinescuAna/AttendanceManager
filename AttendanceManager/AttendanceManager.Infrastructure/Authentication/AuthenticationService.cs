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
            var result = await _mediator.Send(new GetUserByEmailQuery() { Email = request.Email});
            
            if (result == null)
            {
                throw new Exception($"User with {request.Email} not found.");
            }

            if (result.Password != request.Password)
            {
                throw new Exception($"Credentials for '{request.Email} aren't valid'.");
            }

            var jwtSecurityToken = await GenerateToken(result);

            AuthenticationResponse response = new AuthenticationResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = result.Email,
                Fullname = result.FullName,
                Code = result.Code
            };

            return response;
        }
        private Task<JwtSecurityToken> GenerateToken(UserVm user)
        {

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("Code", String.IsNullOrEmpty(user.Code)? string.Empty:user.Code )
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return Task.FromResult(jwtSecurityToken);

        }
    }
}
