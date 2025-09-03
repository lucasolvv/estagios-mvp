using AutoMapper;
using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;
using PlataformaEstagios.Domain.Entities;
using System.Globalization;

namespace PlataformaEstagios.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            // Converters
            CreateMap<DateTime?, DateOnly?>()
                .ConvertUsing(src => src.HasValue ? DateOnly.FromDateTime(src.Value) : (DateOnly?)null);

            CreateMap<string?, DateOnly?>()
                .ConvertUsing(s => string.IsNullOrWhiteSpace(s) ? null : DateOnly.Parse(s!, CultureInfo.InvariantCulture));

            RequestToDomain();
            DomainToResponse();
        }

        private void RequestToDomain()
        {
            // User (igual ao que já tinha)
            CreateMap<RequestCreateUserJson, User>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.UserIdentifier, o => o.Ignore())
                .ForMember(d => d.Active, o => o.Ignore())
                .ForMember(d => d.CreatedOn, o => o.Ignore())
                .ForMember(d => d.Password, o => o.Ignore()) // hash no use case
                .ForMember(d => d.Nickname, o => o.MapFrom(s => s.Nickname))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.UserType, o => o.MapFrom(s => s.UserType))
                .ForMember(d => d.UserTypeId, o => o.Ignore()); // setado no use case
                                                               

            // Candidate: BirthDate (DateTime? -> DateOnly?) usa o converter acima automaticamente
            CreateMap<RequestCandidateJson, Candidate>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CandidateIdentifier, o => o.Ignore())
                .ForMember(d => d.UserIdentifier, o => o.Ignore())
                .ForMember(d => d.Active, o => o.Ignore())
                .ForMember(d => d.CreatedOn, o => o.Ignore())
                .ForMember(d => d.Name, o => o.MapFrom(s => s.FullName))
                .ForMember(d => d.BirthDate, o => o.MapFrom(s => s.BirthDate)) // DateTime? -> DateOnly?
                .ForMember(d => d.CourseName, o => o.MapFrom(s => s.CourseName))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                .ForMember(d => d.Applications, o => o.Ignore());

            // Enterprise
            CreateMap<RequestEnterpriseJson, Enterprise>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.EnterpriseIdentifier, o => o.Ignore())
                .ForMember(d => d.UserIdentifier, o => o.Ignore())
                .ForMember(d => d.Active, o => o.Ignore())
                .ForMember(d => d.CreatedOn, o => o.Ignore())
                .ForMember(d => d.EnterpriseName, o => o.MapFrom(s => s.TradeName))
                .ForMember(d => d.Cnpj, o => o.MapFrom(s => s.Cnpj))
                .ForMember(d => d.ActivityArea, o => o.MapFrom(s => s.ActivityArea))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                .ForMember(d => d.Vacancies, o => o.Ignore());

            // Address
            CreateMap<RequestAddressJson, Address>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.AddressIdentifier, o => o.Ignore())
                .ForMember(d => d.Active, o => o.Ignore())
                .ForMember(d => d.CreatedOn, o => o.Ignore());

            //Vacancy
            CreateMap<RequestCreateVacancyJson, Vacancy>()
                .ForMember(d => d.VacancyIdentifier, o => o.Ignore())
                .ForMember(d => d.EnterpriseIdentifier, o => o.MapFrom(s => s.EnterpriseIdentifier))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.Location, o => o.MapFrom(s => s.Location))
                .ForMember(d => d.JobFunction, o => o.MapFrom(s => s.JobFunction))
                .ForMember(d => d.RequiredSkillsCsv, o => o.Ignore())   // setado no Use Case
                .ForMember(d => d.PublishedAtUtc, o => o.Ignore())      // setado no Use Case
                .ForMember(d => d.ExpiresAtUtc, o => o.MapFrom(s => s.ExpiresAtUtc))
                .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IsActive))
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.Applications, o => o.Ignore());


        }

        private void DomainToResponse()
        {
            CreateMap<Vacancy, ResponseCreateVacancyJson>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title ?? string.Empty))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.Location, o => o.MapFrom(s => s.Location))
                .ForMember(d => d.JobFunction, o => o.MapFrom(s => s.JobFunction))
                .ForMember(d => d.RequiredSkills, o => o.MapFrom(s =>
                    string.IsNullOrWhiteSpace(s.RequiredSkillsCsv)
                        ? new List<string>()
                        : s.RequiredSkillsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList()))
                .ForMember(d => d.PublishedAtUtc, o => o.MapFrom(s => s.PublishedAtUtc))
                .ForMember(d => d.ExpiresAtUtc, o => o.MapFrom(s => s.ExpiresAtUtc))
                .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IsActive))
                .ForMember(d => d.UpdatedAt, o => o.MapFrom(s => s.UpdatedAt ?? DateTime.UtcNow));

            // DomainToResponse()
            CreateMap<Vacancy, ResponseVacancyListItem>()
                .ForCtorParam("VacancyIdentifier", o => o.MapFrom(s => s.VacancyIdentifier))
                .ForCtorParam("Title", o => o.MapFrom(s => s.Title ?? string.Empty))
                .ForCtorParam("OpenedAt", o => o.MapFrom(s => s.PublishedAtUtc ?? s.UpdatedAt ?? DateTime.UtcNow))
                .ForCtorParam("Applicants", o => o.MapFrom(s => s.Applications != null ? s.Applications.Count : 0))
                .ForCtorParam("Status", o => o.MapFrom(s => s.IsActive ? "Ativa" : "Encerrada"));

        }
    }
}
