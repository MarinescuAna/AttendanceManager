using AttendanceManager.Application.Contracts.UnitOfWork;
using AttendanceManager.Persistance.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AttendanceManager.Persistance
{
    public static class PersistanceServiceRegistration
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
        {

            //setting up the EF connection
            services.AddDbContext<AttendanceManagerDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AttendanceManagerConnectionString"),
                b => b.MigrationsAssembly(typeof(AttendanceManagerDbContext).Assembly.FullName)));

            services.AddScoped<IUnitOfWork,UnitOfWork>();

            return services;
        }
    }
}
