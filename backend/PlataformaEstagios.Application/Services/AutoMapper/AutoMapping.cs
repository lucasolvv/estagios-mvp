using AutoMapper;
using PlataformaEstagios.Communication.Requests;

namespace PlataformaEstagios.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToDomain();
            //DomainToResponse();
        }

        private void RequestToDomain()
        {
            CreateMap<ResquestCreateUserJson, Domain.Entities.User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserIdentifier, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.UserType, opt => opt.Ignore())
                .ForMember(dest => dest.Active, opt => opt.Ignore())
                .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.Nickname))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
                
        }
    }
}
