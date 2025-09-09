namespace PlataformaEstagios.Application.UseCases.Vacancy.Update
{
    public interface IUpdateVacancyUseCase
    {
        Task<bool> UpdateVacancyAsync(Guid enterpriseId, Guid vacancyId, Communication.Requests.RequestUpdateVacancyJson request, CancellationToken ct);
    }
}
