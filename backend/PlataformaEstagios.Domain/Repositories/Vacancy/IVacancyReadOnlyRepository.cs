namespace PlataformaEstagios.Domain.Repositories.Vacancy
{
    public interface IVacancyReadOnlyRepository
    {
        Task<IReadOnlyList<Entities.Vacancy>> ListByEnterpriseAsync(Guid enterpriseId, CancellationToken ct);
    }
}
