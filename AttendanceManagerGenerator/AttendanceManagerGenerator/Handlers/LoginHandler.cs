using AttendanceManagerGenerator.Modules;

namespace AttendanceManagerGenerator.Handlers
{
    public class LoginHandler
    {
        private readonly RequestHandler _handler;
        public LoginHandler(HttpClient http)
        {
            _handler = new RequestHandler(http, "account");
        }

        public async Task<AuthenticationResponse?> LoginAsync(string email, string password) 
           => await _handler.PostAsync<UserPostVm, AuthenticationResponse>("authenticate", new UserPostVm()
        {
            Email = email!,
            Password = password!
        });
    }
}
