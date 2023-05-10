using AttendanceManager.Application.Models.Authentication;

namespace AttendanceManager.Application.Contracts.Infrastructure.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
    }
}
