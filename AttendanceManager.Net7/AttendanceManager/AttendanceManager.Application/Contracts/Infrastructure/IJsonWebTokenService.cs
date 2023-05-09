using AttendanceManager.Application.Modules.Authentication;

namespace AttendanceManager.Application.Contracts.Infrastructure
{
    public interface IJsonWebTokenService
    {
        GenericToken GenerateAccessToken(string email, string name, string role, string code);
        Task<GenericToken> GenerateRefreshToken();
    }
}
