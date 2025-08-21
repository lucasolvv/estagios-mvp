using AutoMapper;
using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Domain.Entities;

namespace PlataformaEstagios.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToDomain();
        }

        private void RequestToDomain()
        {
            // User (top-level)
            CreateMap<RequestCreateUserJson, User>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.UserIdentifier, o => o.Ignore())
                .ForMember(d => d.Active, o => o.Ignore())
                .ForMember(d => d.CreatedOn, o => o.Ignore())
                .ForMember(d => d.Password, o => o.Ignore()) // hashed in use case
                .ForMember(d => d.Nickname, o => o.MapFrom(s => s.Nickname))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.UserType, o => o.MapFrom(s => s.UserType));

            // Candidate (nested payload)
            CreateMap<RequestCandidateJson, Candidate>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CandidateIdentifier, o => o.Ignore())
                .ForMember(d => d.UserIdentifier, o => o.Ignore()) // set in use case
                .ForMember(d => d.Active, o => o.Ignore())
                .ForMember(d => d.CreatedOn, o => o.Ignore())
                .ForMember(d => d.Name, o => o.MapFrom(s => s.FullName))
                .ForMember(d => d.BirthDate, o => o.MapFrom(s => s.BirthDate))
                .ForMember(d => d.CourseName, o => o.MapFrom(s => s.CourseName))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                .ForMember(d => d.Applications, o => o.Ignore());

            // Enterprise (nested payload)
            CreateMap<RequestEnterpriseJson, Enterprise>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.EnterpriseIdentifier, o => o.Ignore())
                .ForMember(d => d.UserIdentifier, o => o.Ignore()) // set in use case
                .ForMember(d => d.Active, o => o.Ignore())
                .ForMember(d => d.CreatedOn, o => o.Ignore())
                .ForMember(d => d.EnterpriseName, o => o.MapFrom(s => s.TradeName))
                .ForMember(d => d.Cnpj, o => o.MapFrom(s => s.Cnpj))
                .ForMember(d => d.ActivityArea, o => o.MapFrom(s => s.ActivityArea))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                .ForMember(d => d.Vacancies, o => o.Ignore());

            // Address (shared)
            CreateMap<RequestAddressJson, Address>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.AddressIdentifier, o => o.Ignore())
                .ForMember(d => d.Active, o => o.Ignore())
                .ForMember(d => d.CreatedOn, o => o.Ignore());
        }
    }
}
