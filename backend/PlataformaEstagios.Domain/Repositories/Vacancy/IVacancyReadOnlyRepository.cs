namespace PlataformaEstagios.Domain.Repositories.Vacancy
{
    public interface IVacancyReadOnlyRepository
    {
        Task<IReadOnlyList<Entities.Vacancy>> GetActiveVacanciesForEnterpriseAsync(Guid enterpriseId, CancellationToken ct = default);
        Task<IReadOnlyList<Entities.Vacancy>> GetActiveVacanciesForCandidateAsync(CancellationToken ct = default);
        Task<Entities.Vacancy?> GetVacancyByIdForCandidateAsync(Guid vacancyId, CancellationToken ct = default);
        Task<Entities.Vacancy?> GetVacancyByIdForEnterpriseAsync(Guid enterpriseId, Guid vacancyId, CancellationToken ct = default);
        //Task<EnterpriseHomeStatsResponse> GetStatsAsync(Guid enterpriseId, CancellationToken ct = default);
        //Task<IReadOnlyList<RecentCandidateResponse>> GetRecentCandidatesAsync(Guid enterpriseId, int take, CancellationToken ct = default);
    }
}
