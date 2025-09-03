namespace PlataformaEstagios.Application.UseCases.Enterprise.GetVacancies
{
    public interface IGetVacanciesUseCase
    {
        Task<IReadOnlyList<Communication.Responses.ResponseVacancyListItem>> ExecuteAsync(Guid enterpriseId, CancellationToken ct);
    }
}
