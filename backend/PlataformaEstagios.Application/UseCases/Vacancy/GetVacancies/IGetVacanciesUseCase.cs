namespace PlataformaEstagios.Application.UseCases.Vacancy.GetVacancies
{
    public interface IGetVacanciesUseCase
    {
        Task<IReadOnlyList<Communication.Responses.ResponseVacancyListItem>> ExecuteAsync(Guid enterpriseId, CancellationToken ct);
        Task<IReadOnlyList<Communication.Responses.ResponseVacancyListItem>> ExecuteAsync(CancellationToken ct);
        Task<Communication.Responses.ResponseGetVacancyJson> GetByIdAsync(Guid enterpriseId, Guid vacancyId, CancellationToken ct);
    }
}
