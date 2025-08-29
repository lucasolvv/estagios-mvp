namespace PlataformaEstagios.Domain.Repositories.Vacancy
{
    public interface IVacancyWriteOnlyRepository
    {
        Task AddAsync(Entities.Vacancy entity, CancellationToken ct);
    }
}
