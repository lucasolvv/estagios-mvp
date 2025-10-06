namespace PlataformaEstagios.Domain.Repositories.Interview
{
    public interface IInterviewReadOnlyRepository
    {
        Task<bool> ExistsSameStartAsync(Guid applicationId, DateTimeOffset startAt);
        // (depois você pode adicionar: GetByApplicationIdAsync, UpcomingByEnterpriseAsync, etc.)
        Task<IReadOnlyList<Domain.Entities.Interview>?> GetByApplicationIdAsync(Guid applicationId);
    }
}
