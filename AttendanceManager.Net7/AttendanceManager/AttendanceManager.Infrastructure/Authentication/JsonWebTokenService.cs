using AttendanceManager.Application.Contracts.Infrastructure.Authentication;
using AttendanceManager.Application.Features.User.Queries.GetRefreshTokens;
using AttendanceManager.Application.Models.Authentication;
using AttendanceManager.Application.Modules.Authentication;
using AttendanceManager.Domain.Common;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AttendanceManager.Infrastructure.Authentication
{
    public sealed class JsonWebTokenService : IJsonWebTokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IMediator _mediator;

        public JsonWebTokenService(IMediator mediator, IOptions<JwtSettings> jwtSettings)
        {
            _mediator = mediator;
            _jwtSettings = jwtSettings.Value;
        }

        public GenericToken GenerateAccessToken(string email, string name, string role, string code)
        {
            // define a bunch of clams
            var claims = new[]
            {
                new Claim(Constants.Claim_Name_Email, email),
                new Claim(Constants.Claim_Name_Name, name),
                new Claim(Constants.Claim_Name_Role, role),
                new Claim(Constants.Claim_Name_Code, string.IsNullOrEmpty(code)? string.Empty:code )
            };
            var expirationDate = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);
            // grab the security key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            //define a credential object base on the security key definded above
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

            //deifine the token object
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expirationDate,
                signingCredentials: signingCredentials);

            return new()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Expiration = expirationDate
            };
        }

        public async Task<GenericToken> GenerateRefreshToken()
        {
            var refreshTokens = await _mediator.Send(new GetRefreshTokensQuery());
            var newRefreshToken = string.Empty;

            if (refreshTokens != null)
            {
                do
                {
                    newRefreshToken = GenerateToken();
                } while (refreshTokens.Any(c => c.Equals(newRefreshToken)));
            }
            else
            {
                newRefreshToken = GenerateToken();
            }
            return new()
            {
                Expiration = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                Token = newRefreshToken
            };

        }

        private string GenerateToken()
            => new string(Enumerable.Repeat(Constants.CharsString, Constants.RefreshTokenLength).Select(s => s[new Random().Next(s.Length)]).ToArray());
    }
}
