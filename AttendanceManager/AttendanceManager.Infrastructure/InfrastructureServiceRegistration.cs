using AttendanceManager.Application.Contracts.Authentication;
using AttendanceManager.Application.Contracts.Mail;
using AttendanceManager.Infrastructure.Authentication;
using AttendanceManager.Infrastructure.Mail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace AttendanceManager.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Application.Models.Authentication.JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<Application.Models.Mail.MailSettings>(configuration.GetSection("MailKit"));

            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IMailService, MailService>();

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
                  o.RequireHttpsMetadata = false;
                  o.SaveToken = false;
                  o.TokenValidationParameters = new TokenValidationParameters
                  {
                      // ensure the token was issued by a trusted authorization server
                      ValidateIssuerSigningKey = true,
                      ValidateIssuer = true,
                      ValidIssuer = configuration["JwtSettings:Issuer"],

                      // ensure the token hasn't expired
                      ValidateLifetime = true,
                      RequireExpirationTime = true,

                      // clock skew compensates for server time drift(should be 5min or less)
                      // set clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                      ClockSkew = TimeSpan.Zero,

                      // ensure the token audience matches out audience value
                      ValidateAudience = true,
                      ValidAudience = configuration["JwtSettings:Audience"],

                      // this key is required because it helps the app and authorization server to recognize the token
                      // this is called a symmetric key
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
                  };
              });
        }
    }
}
