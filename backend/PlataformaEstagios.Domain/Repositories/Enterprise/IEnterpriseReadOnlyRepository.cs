namespace PlataformaEstagios.Domain.Repositories.Enterprise
{
    public interface IEnterpriseReadOnlyRepository
    {
        Task<bool> ExistsAsync(Guid enterpriseId, CancellationToken ct);
    }
}
