using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagios.Application.UseCases.Vacancy.Create
{
    public interface ICreateVacancyUseCase
    {
        Task<ResponseCreateVacancyJson> ExecuteAsync(RequestCreateVacancyJson request, CancellationToken ct);

    }
}
