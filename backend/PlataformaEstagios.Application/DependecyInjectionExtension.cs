using Microsoft.Extensions.DependencyInjection;
using PlataformaEstagios.Application.Services.AutoMapper;
using PlataformaEstagios.Application.UseCases.User.Create;


namespace PlataformaEstagios.Application
{
    public static class DependecyInjectionExtension
    {
        public static void AddAppDependencies(this IServiceCollection services)
        {
            AddUseCases(services);
            AddAutoMapper(services);

        }
        private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapping));

        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
        }
    }
}
