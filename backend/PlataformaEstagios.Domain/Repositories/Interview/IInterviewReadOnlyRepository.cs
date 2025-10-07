namespace PlataformaEstagios.Domain.Repositories.Interview
{
    public interface IInterviewReadOnlyRepository
    {
        Task<bool> ExistsSameStartAsync(Guid applicationId, DateTimeOffset startAt);
        // (depois você pode adicionar: GetByApplicationIdAsync, UpcomingByEnterpriseAsync, etc.)
        Task<IReadOnlyList<Entities.Interview>?> GetByApplicationIdAsync(Guid applicationId);
        Task<IReadOnlyList<Entities.Interview>?> GetByEnterpriseIdAsync(Guid enterpriseId);
    }
}
