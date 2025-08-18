using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlataformaEstagios.Domain.Repositories;
using PlataformaEstagios.Domain.Repositories.User;
using PlataformaEstagios.Infrastructure.DataAccess;
using PlataformaEstagios.Infrastructure.Repositories;

namespace PlataformaEstagios.Infrastructure
{
    public static class DependecyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext_PostGreSQL(services, configuration);
            AddRepositories(services);
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        }

        private static void AddDbContext_PostGreSQL(IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("Default")
               ?? throw new InvalidOperationException("Connection string 'Default' not found.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connString, npgsql =>
                {
                    // Optional: keep migrations in Infrastructure
                    npgsql.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name);
                }));
        }
    }
}
