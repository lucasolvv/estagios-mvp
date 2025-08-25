using PlataformaEstagios.Domain.Entities;

namespace PlataformaEstagios.Application.Services.Auth
{
    public interface IJwtTokenGenerator
    {
        string Generate(User user, out DateTime expiresAtUtc);
    }
}
