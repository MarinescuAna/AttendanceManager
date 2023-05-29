
using AttendanceManager.Application.Contracts.Infrastructure.Authentication;
using AttendanceManager.Application.Contracts.Infrastructure.Logging;
using AttendanceManager.Application.Contracts.Infrastructure.Mail;
using AttendanceManager.Application.Contracts.Infrastructure.Singleton;
using AttendanceManager.Infrastructure.Authentication;
using AttendanceManager.Infrastructure.Logging;
using AttendanceManager.Infrastructure.Mail;
using AttendanceManager.Infrastructure.Singleton;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AttendanceManager.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IJsonWebTokenService, JsonWebTokenService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ILoggingService, LoggingService>();
            services.AddSingleton<IReportSingleton,ReportSingleton>();

            AddAuthentication(services, configuration);
        }
        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(o =>
              {
                  o.TokenValidationParameters = new TokenValidationParameters
                  {
                      // ensure the token was issued by a trusted authorization server
                      ValidateIssuerSigningKey = true,
                      ValidateIssuer = true,
                      ValidIssuer = configuration["JwtSettings:Issuer"],

                      // ensure the token hasn't expired
                      ValidateLifetime = true,
                      RequireExpirationTime = true,

                      // ensure the token audience matches out audience value
                      ValidateAudience = true,
                      ValidAudience = configuration["JwtSettings:Audience"],

                      // this key is required because it helps the app and authorization server to recognize the token
                      // this is called a symmetric key
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)),
                  };
              });
        }
    }
}
