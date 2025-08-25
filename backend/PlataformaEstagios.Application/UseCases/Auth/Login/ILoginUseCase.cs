using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagios.Application.UseCases.Auth.Login
{
    public interface ILoginUseCase
    {
        Task<ResponseLoginJson> ExecuteAsync(RequestLoginJson request, CancellationToken ct = default);
    }
}
