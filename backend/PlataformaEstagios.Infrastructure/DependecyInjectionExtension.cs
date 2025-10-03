using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlataformaEstagios.Domain.Repositories;
using PlataformaEstagios.Domain.Repositories.Application;
using PlataformaEstagios.Domain.Repositories.Candidate;
using PlataformaEstagios.Domain.Repositories.Enterprise;
using PlataformaEstagios.Domain.Repositories.Interview;
using PlataformaEstagios.Domain.Repositories.User;
using PlataformaEstagios.Domain.Repositories.Vacancy;
using PlataformaEstagios.Infrastructure.DataAccess;
using PlataformaEstagios.Infrastructure.Repositories;
using PlataformaEstagios.Infrastructure.Storage;

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
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<ICandidateWriteOnlyRepository, CandidateRepository>();
            services.AddScoped<ICandidateReadOnlyRepository, CandidateRepository>();
            services.AddScoped<IEnterpriseWriteOnlyRepository, EnterpriseRepository>();
            services.AddScoped<IEnterpriseReadOnlyRepository, EnterpriseRepository>();
            services.AddScoped<IVacancyReadOnlyRepository, VacancyRepository>();
            services.AddScoped<IVacancyWriteOnlyRepository, VacancyRepository>();
            services.AddScoped<IApplicationWriteOnlyRepository, ApplicationRepository>();
            services.AddScoped<IApplicationReadOnlyRepository, ApplicationRepository>();
            services.AddScoped<IInterviewReadOnlyRepository, InterviewRepository>();
            services.AddScoped<IInterviewWriteOnlyRepository, InterviewRepository>();
            services.AddScoped<IFileStorage, LocalFileStorage>();

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
