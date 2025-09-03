namespace PlataformaEstagios.Domain.Repositories.Vacancy
{
    public interface IVacancyReadOnlyRepository
    {
        Task<IReadOnlyList<Entities.Vacancy>> GetActiveForEnterpriseAsync(Guid enterpriseId, CancellationToken ct = default);
        //Task<EnterpriseHomeStatsResponse> GetStatsAsync(Guid enterpriseId, CancellationToken ct = default);
        //Task<IReadOnlyList<RecentCandidateResponse>> GetRecentCandidatesAsync(Guid enterpriseId, int take, CancellationToken ct = default);
    }
}
