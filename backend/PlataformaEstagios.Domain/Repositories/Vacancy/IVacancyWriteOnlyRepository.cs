namespace PlataformaEstagios.Domain.Repositories.Vacancy
{
    public interface IVacancyWriteOnlyRepository
    {
        Task AddVacancyAsync(Entities.Vacancy entity, CancellationToken ct);
        Task UpdateVacancyAsync(Entities.Vacancy entity, CancellationToken ct);
    }
}
