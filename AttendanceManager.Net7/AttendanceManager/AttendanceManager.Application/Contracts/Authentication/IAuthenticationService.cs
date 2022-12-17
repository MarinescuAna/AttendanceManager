using AttendanceManager.Application.Models.Authentication;

namespace AttendanceManager.Application.Contracts.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
    }
}
