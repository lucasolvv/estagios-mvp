namespace PlataformaEstagios.Application.UseCases.Enterprise.UpdateVacancies
{
    public interface IUpdateVacancyUseCase
    {
        Task<bool> ExecuteAsync(Guid enterpriseId, Guid vacancyId, Communication.Requests.RequestUpdateVacancyJson request, CancellationToken ct);
    }
}
