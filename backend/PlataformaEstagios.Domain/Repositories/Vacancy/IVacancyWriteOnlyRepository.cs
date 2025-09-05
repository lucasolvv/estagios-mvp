namespace PlataformaEstagios.Domain.Repositories.Vacancy
{
    public interface IVacancyWriteOnlyRepository
    {
        Task AddAsync(Entities.Vacancy entity, CancellationToken ct);
        Task UpdateAsync(Entities.Vacancy entity, CancellationToken ct);
    }
}
