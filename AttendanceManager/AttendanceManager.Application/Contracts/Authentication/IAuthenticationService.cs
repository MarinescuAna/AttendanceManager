using AttendanceManager.Application.Models.Authentication;
using System.Threading.Tasks;

namespace AttendanceManager.Application.Contracts.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
    }
}
