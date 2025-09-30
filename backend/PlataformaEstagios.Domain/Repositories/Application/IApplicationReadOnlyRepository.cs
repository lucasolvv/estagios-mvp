using PlataformaEstagios.Domain.Entities;

namespace PlataformaEstagios.Domain.Repositories.Application
{
    public interface IApplicationReadOnlyRepository 
    {
        Task<bool> ExistsAsync(Guid vacancyId, Guid candidateId);
        Task<IReadOnlyList<Domain.Entities.Application>> GetRecentApplicationsByCandidateIdAsync(Guid candidateId);
        Task<IReadOnlyList<Domain.Entities.Application>> GetRecentApplicationsByEnterpriseIdAsync(Guid enterpriseId);
        Task<Entities.Application> GetApplicationByCandidateIdAsync(Guid enterpriseId);
    }
}
