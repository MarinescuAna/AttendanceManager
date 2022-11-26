using AttendanceManager.Application.Contracts.Persistance;
using AttendanceManager.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AttendanceManager.Persistance
{
    public static class PersistanceServiceRegistration
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
        {

            //setting up the EF connection
            services.AddDbContext<AttendanceManagerDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AttendanceManagerConnectionString"),
                b => b.MigrationsAssembly(typeof(AttendanceManagerDbContext).Assembly.FullName)));

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();
            services.AddScoped<IUserSpecializationRepository, UserSpecializationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
