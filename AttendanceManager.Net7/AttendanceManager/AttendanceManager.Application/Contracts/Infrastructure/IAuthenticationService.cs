using AttendanceManager.Application.Models.Authentication;

namespace AttendanceManager.Application.Contracts.Infrastructure
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
    }
}
