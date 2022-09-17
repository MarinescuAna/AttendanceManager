using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AttendanceManager.Persistance
{
    public static class PersistanceServiceRegistration
    {
        public static void AddDbConnectionServices(this IServiceCollection services, IConfiguration configuration)
        {

            //setting up the EF connection
            services.AddDbContext<AttendanceManagerDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AttendanceManagerConnectionString"),
                b => b.MigrationsAssembly(typeof(AttendanceManagerDbContext).Assembly.FullName)));
        }
    }
}
