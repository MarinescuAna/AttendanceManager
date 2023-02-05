using AttendanceManager.Application.Models.Authentication;
using AttendanceManager.Application.Modules.Authentication;
using AttendanceManager.Core.Shared;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AttendanceManager.Infrastructure.Authentication
{
    public sealed class JsonWebTokenService : Application.Contracts.Authentication.IJsonWebTokenService
    {
        private readonly JwtSettings _jwtSettings;

        public JsonWebTokenService(IOptions<JwtSettings> jwtSettings)
        {
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
            var expirationDate = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);
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

        public GenericToken GenerateRefreshToken()
        {
            var randomNumber = new byte[60];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return new()
            {
                Expiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                Token = Convert.ToBase64String(randomNumber)
            };
        }
    }
}
