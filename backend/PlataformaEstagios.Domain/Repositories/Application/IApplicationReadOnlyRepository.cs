using PlataformaEstagios.Domain.Entities;

namespace PlataformaEstagios.Domain.Repositories.Application
{
    public interface IApplicationReadOnlyRepository 
    {
        Task<bool> ExistsAsync(Guid vacancyId, Guid candidateId);
        Task<IReadOnlyList<Domain.Entities.Application>> GetRecentApplicationsByCandidateIdAsync(Guid candidateId);
        Task<IReadOnlyList<Domain.Entities.Application>> GetRecentApplicationsByEnterpriseIdAsync(Guid enterpriseId);
        Task<Entities.Application?> GetApplicationByApplicationIdAsync(Guid applicationId);
        Task<Entities.Application?> GetByIdAsync(Guid applicationId, CancellationToken ct = default);
    }
}
