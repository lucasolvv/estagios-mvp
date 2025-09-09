using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagios.Application.UseCases.Vacancy.Get
{
    public interface IGetVacanciesUseCase
    {
        Task<IReadOnlyList<ResponseVacancyListItem>> GetAllActiveVacanciesForEnterpriseAsync(Guid enterpriseId, CancellationToken ct);
        Task<IReadOnlyList<ResponseVacancyListItem>> GetAllActiveVacanciesForCandidateAsync(CancellationToken ct);
        Task<ResponseGetVacancyJson> GetVacancyByIdForEnterpriseAsync(Guid enterpriseId, Guid vacancyId, CancellationToken ct);
        Task<ResponseGetVacancyToApplicationJson> GetVacancyByIdForCandidateAsync(Guid vacancyId, CancellationToken ct);
    }
}
