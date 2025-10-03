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

            CreateMap<DateOnly?, DateTime?>()
                .ConvertUsing(src => src.HasValue ? src.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null);
            CreateMap<Address, Communication.AddressDto>();

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
                .ForMember(d => d.Applications, o => o.Ignore())
                .ForMember(d => d.ProfilePicturePath, o => o.Ignore())
                .ForMember(d => d.ResumePath, o => o.Ignore());

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

            CreateMap<RequestUpdateVacancyJson, Vacancy>()
                .ForMember(d => d.VacancyIdentifier, o => o.Ignore())
                .ForMember(d => d.EnterpriseIdentifier, o => o.Ignore())
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.Location, o => o.MapFrom(s => s.Location))
                .ForMember(d => d.JobFunction, o => o.MapFrom(s => s.JobFunction))
                .ForMember(d => d.RequiredSkillsCsv, o => o.Ignore())   // setado no Use Case
                .ForMember(d => d.PublishedAtUtc, o => o.Ignore())      // não pode ser alterado
                .ForMember(d => d.ExpiresAtUtc, o => o.MapFrom(s => s.ExpiresAtUtc))
                .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IsActive))
                .ForMember(d => d.UpdatedAt, o => o.Ignore())           // setado no Use Case
                .ForMember(d => d.Applications, o => o.Ignore());

            CreateMap<RequestUpdateCandidateProfileJson, Candidate>()
               //.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null))
               .ForMember(d => d.Name, o => o.MapFrom(s => s.FullName))
               .ForMember(d => d.BirthDate, o => o.MapFrom(s => s.BirthDate)) // DateOnly? já está no DTO
               .ForMember(d => d.CourseName, o => o.MapFrom(s => s.Course))
               .ForMember(d => d.BioResume, o => o.MapFrom(s => s.BioResume))
               // Não mapeie arquivos aqui; use case já trata
               .ForMember(d => d.ProfilePicturePath, o => o.Ignore())
               .ForMember(d => d.ResumePath, o => o.Ignore())
               .ForMember(d => d.Applications, o => o.Ignore());
            
            CreateMap<RequestUpdateAddressJson, Address>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // Interview (criar agendamento)
            CreateMap<RequestCreateScheduleInterviewJson, Interview>()
                .ForMember(d => d.InterviewIdentifier, o => o.Ignore())   // Gerado no UseCase
                .ForMember(d => d.ApplicationIdentifier, o => o.Ignore()) // Vem da rota (UseCase)
                .ForMember(d => d.Application, o => o.Ignore())           // nav
                .ForMember(d => d.StartAt, o => o.MapFrom(s => s.StartAt))
                .ForMember(d => d.DurationMinutes, o => o.MapFrom(s => s.DurationMinutes))
                .ForMember(d => d.Location, o => o.MapFrom(s => s.Location))
                .ForMember(d => d.MeetingLink, o => o.MapFrom(s => s.MeetingLink))
                .ForMember(d => d.Notes, o => o.MapFrom(s => s.Notes))
                .ForMember(d => d.CreatedAt, o => o.Ignore())             // setado pelo banco/UseCase
                .ForMember(d => d.UpdatedAt, o => o.Ignore());



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
                .ForMember(d => d.VacancyIdentifier, o => o.MapFrom(s => s.VacancyIdentifier))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title ?? string.Empty))
                .ForMember(d => d.OpenedAt, o => o.MapFrom(s => s.PublishedAtUtc))
                .ForMember(d => d.Applicants, o => o.MapFrom(s => s.Applications != null ? s.Applications.Count : 0))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.IsActive ? "Ativa" : "Inativa"))
                .ForMember(d => d.EnterpriseName, o => o.Ignore());


            CreateMap<Vacancy, ResponseGetVacancyJson>()
                .ForMember(d => d.VacancyIdentifier, o => o.MapFrom(s => s.VacancyIdentifier))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title ?? string.Empty))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.Location, o => o.MapFrom(s => s.Location))
                .ForMember(d => d.JobFunction, o => o.MapFrom(s => s.JobFunction))
                .ForMember(d => d.RequiredSkills, o => o.MapFrom(s =>
                    string.IsNullOrWhiteSpace(s.RequiredSkillsCsv)
                        ? new List<string>()
                        : s.RequiredSkillsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList()))
                .ForMember(d => d.ExpiresAtUtc, o => o.MapFrom(s => s.ExpiresAtUtc))
                .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IsActive));

            CreateMap<Vacancy, ResponseGetVacancyToApplicationJson>()
                .ForMember(destinationMember => destinationMember.VacancyIdentifier, opt => opt.MapFrom(src => src.VacancyIdentifier))
                .ForMember(destinationMember => destinationMember.Title, opt => opt.MapFrom(src => src.Title ?? string.Empty))
                .ForMember(destinationMember => destinationMember.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(destinationMember => destinationMember.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(destinationMember => destinationMember.JobFunction, opt => opt.MapFrom(src => src.JobFunction))
                .ForMember(destinationMember => destinationMember.RequiredSkills, opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.RequiredSkillsCsv)
                        ? new List<string>()
                        : src.RequiredSkillsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList()))
                .ForMember(destinationMember => destinationMember.OpenedAt, opt => opt.MapFrom(src => src.PublishedAtUtc))
                .ForMember(destinationMember => destinationMember.ExpiresAtUtc, opt => opt.MapFrom(src => src.ExpiresAtUtc));

            CreateMap<Domain.Entities.Application, ResponseGetApplicationJson>()
                .ForMember(destinationMember => destinationMember.TituloVaga, opt => opt.MapFrom(src => src.Vacancy != null ? src.Vacancy.Title ?? string.Empty : string.Empty))
                .ForMember(destinationMember => destinationMember.NomeEmpresa, opt => opt.Ignore())
                .ForMember(destinationMember => destinationMember.DataCandidatura, opt => opt.MapFrom(src => src.ApplicationDate))
                .ForMember(destinationMember => destinationMember.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(destinationMember => destinationMember.ApplicationIdentifier, opt => opt.MapFrom(src => src.ApplicationIdentifier))
                .ForMember(destinationMember => destinationMember.VacancyIdentifier, opt => opt.MapFrom(src => src.VacancyId))
                .ForMember(destinationMember => destinationMember.CandidateIdentifier, opt => opt.MapFrom(src => src.CandidateIdentifier))
                .ForMember(destinationMember => destinationMember.NomeCandidato, opt => opt.Ignore())
                .ForMember(destinationMember => destinationMember.NomeCurso, opt => opt.Ignore())
                .ForMember(destinationMember => destinationMember.StatusEnum, opt => opt.MapFrom(src => src.Status))
                .ForMember(destinationMember => destinationMember.DataCadastroVaga, opt => opt.Ignore());


            CreateMap<Domain.Entities.Candidate, ResponseGetCandidateProfileJson>()
                .ForMember(destinationMember => destinationMember.CandidateId, opt => opt.MapFrom(src => src.CandidateIdentifier))
                .ForMember(destinationMember => destinationMember.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(destinationMember => destinationMember.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(destinationMember => destinationMember.BioResume, opt => opt.MapFrom(src => src.BioResume))
                .ForMember(destinationMember => destinationMember.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(destinationMember => destinationMember.ProfilePicturePath, opt => opt.MapFrom(src => src.ProfilePicturePath))
                .ForMember(destinationMember => destinationMember.ResumePath, opt => opt.MapFrom(src => src.ResumePath))
                .ForMember(destinationMember => destinationMember.Email, opt => opt.Ignore())
                ;
                


        }
    }
}
