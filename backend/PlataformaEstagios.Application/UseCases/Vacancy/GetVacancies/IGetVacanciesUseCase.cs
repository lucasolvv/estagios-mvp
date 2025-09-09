using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagios.Application.UseCases.Vacancy.GetVacancies
{
    public interface IGetVacanciesUseCase
    {
        Task<IReadOnlyList<ResponseVacancyListItem>> ExecuteAsync(Guid enterpriseId, CancellationToken ct);
        Task<IReadOnlyList<ResponseVacancyListItem>> ExecuteAsync(CancellationToken ct);
        Task<ResponseGetVacancyJson> GetByIdForEnterpriseAsync(Guid enterpriseId, Guid vacancyId, CancellationToken ct);
        Task<ResponseGetVacancyToApplicationJson> GetByIdForCandidateAsync(Guid vacancyId, CancellationToken ct);
    }
}
