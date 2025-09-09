namespace PlataformaEstagios.Domain.Repositories.Application
{
    public interface IApplicationReadOnlyRepository 
    {
        Task<bool> ExistsAsync(Guid vacancyId, Guid candidateId);
    }
}
