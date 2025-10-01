using PlataformaEstagios.Domain.Enums;

namespace PlataformaEstagios.Domain.Repositories.Application
{
    public interface IApplicationWriteOnlyRepository
    {
        Task AddAsync(Domain.Entities.Application application, CancellationToken ct);
        Task UpdateAsync(Entities.Application application, CancellationToken ct = default);
        Task<int> UpdateStatusAsync(Guid applicationId, ApplicationStatus newStatus, CancellationToken ct = default);
    }
}
