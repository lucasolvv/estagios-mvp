namespace PlataformaEstagios.Domain.Repositories.Application
{
    public interface IApplicationWriteOnlyRepository
    {
        Task AddAsync(Domain.Entities.Application application, CancellationToken ct);
    }
}
