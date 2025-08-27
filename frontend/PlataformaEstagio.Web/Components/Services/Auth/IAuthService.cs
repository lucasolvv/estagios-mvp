using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagio.Web.Components.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(RequestLoginJson request);
        Task LogoutAsync();
        Task<string?> GetTokenAsync();
        Task<bool> IsAuthenticatedAsync();
        Task EnsureAuthHeaderAsync(HttpClient http); // anexa "Bearer" no client recebido
        Task<ResponseLoginJson?> GetSessionAsync();  // dados do usuário logado
    }
}
