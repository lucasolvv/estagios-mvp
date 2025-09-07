namespace PlataformaEstagios.Domain.Repositories.Vacancy
{
    public interface IVacancyReadOnlyRepository
    {
        Task<IReadOnlyList<Entities.Vacancy>> GetActiveForEnterpriseAsync(Guid enterpriseId, CancellationToken ct = default);
        Task<IReadOnlyList<Entities.Vacancy>> GetActiveForCandidateAsync(CancellationToken ct = default);
        Task<Entities.Vacancy?> GetByIdForEnterpriseAsync(Guid enterpriseId, Guid vacancyId, CancellationToken ct = default);
        //Task<EnterpriseHomeStatsResponse> GetStatsAsync(Guid enterpriseId, CancellationToken ct = default);
        //Task<IReadOnlyList<RecentCandidateResponse>> GetRecentCandidatesAsync(Guid enterpriseId, int take, CancellationToken ct = default);
    }
}
