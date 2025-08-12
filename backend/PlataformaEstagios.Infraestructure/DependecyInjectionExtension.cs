using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlataformaEstagios.Infrastructure.DataAccess;

namespace PlataformaEstagios.Infraestructure
{
    public static class DependecyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext_PostGreSQL(services, configuration);
            AddRepositories(services);
        }

        private static void AddRepositories(IServiceCollection services) { }

        private static void AddDbContext_PostGreSQL(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbcontext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
