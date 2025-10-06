using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PlataformaEstagios.Application.Services.Auth;
using PlataformaEstagios.Application.Services.AutoMapper;
using PlataformaEstagios.Application.UseCases.Application.Create;
using PlataformaEstagios.Application.UseCases.Application.Get;
using PlataformaEstagios.Application.UseCases.Application.Update;
using PlataformaEstagios.Application.UseCases.Auth.Login;
using PlataformaEstagios.Application.UseCases.Candidate.Get;
using PlataformaEstagios.Application.UseCases.Candidate.UpdateProfile;
using PlataformaEstagios.Application.UseCases.Interview.Create;
using PlataformaEstagios.Application.UseCases.Interview.Get;
using PlataformaEstagios.Application.UseCases.User.Create;
using PlataformaEstagios.Application.UseCases.Vacancy.Create;
using PlataformaEstagios.Application.UseCases.Vacancy.Get;
using PlataformaEstagios.Application.UseCases.Vacancy.Update;

namespace PlataformaEstagios.Application
{
    public static class DependecyInjectionExtension
    {
        public static void AddAppDependencies(this IServiceCollection services)
        {
            AddUseCases(services);
            AddAutoMapper(services);
            AddValidators(services);
            AddAuth(services);
        }
        private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapping));

        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
            services.AddScoped<ILoginUseCase, LoginUseCase>();
            services.AddScoped<ICreateVacancyUseCase, CreateVacancyUseCase>();
            services.AddScoped<IGetVacanciesUseCase, GetVacanciesUseCase>();
            services.AddScoped<IUpdateVacancyUseCase, UpdateVacancyUseCase>();
            services.AddScoped<ICreateApplicationUseCase, CreateApplicationUseCase>();
            services.AddScoped<IGetApplicationUseCase, GetApplicationUseCase>();
            services.AddScoped<IUpdateCandidateProfileUseCase, UpdateCandidateProfileUseCase>();
            services.AddScoped<IGetCandidateUseCase, GetCandidateUseCase>();
            services.AddScoped<IUpdateApplicationUseCase, UpdateApplicationUseCase>();
            services.AddScoped<ICreateInterviewUseCase, CreateInterviewUseCase>();
            services.AddScoped<IGetInterviewUseCase, GetInterviewUseCase>();

        }

        private static void AddValidators(IServiceCollection services)
        {
           services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
           services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
              services.AddValidatorsFromAssemblyContaining<CreateVacancyValidator>();

        }
        private static void AddAuth(IServiceCollection services)
        {
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        }
    }
}
