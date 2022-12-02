using AttendanceManager.Application.Features.User.Commands.UpdateUser;
using AttendanceManager.Application.Features.User.Queries.GetUserByEmail;
using AttendanceManager.Application.Models.Authentication;
using AttendanceManager.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManager.Infrastructure.Authentication
{
    public class AuthenticationService : Application.Contracts.Authentication.IAuthenticationService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(IOptions<JwtSettings> jwtSettings, IMediator mediator, IMapper mapper)
        {
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
            _mediator = mediator;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            //return the current user according to the email
            var result = await _mediator.Send(new GetUserByEmailQuery() { Email = request.Email });

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

            if (!result.AccountConfirmed)
            {
                await _mediator.Send(new UpdateUserCommand() { User = _mapper.Map<User>(result) });
            }

            var jwtSecurityToken = GenerateToken(result);

            AuthenticationResponse response = new AuthenticationResponse
            {
                Token = jwtSecurityToken
                //TODO add refresh token
            };

            return response;
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
