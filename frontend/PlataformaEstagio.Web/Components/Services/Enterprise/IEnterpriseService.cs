using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagio.Web.Components.Services.Enterprise
{
    public interface IEnterpriseService
    {
        Task<List<ResponseVacancyListItem>> GetActiveAsync(Guid enterpriseId);
        Task<ResponseGetVacancyJson> GetByIdAsync(Guid enterpriseId, Guid vacancyId);
        Task<(bool Success, string? Error)> CreateAsync(Guid enterpriseId, RequestCreateVacancyJson dto, CancellationToken ct = default);
        Task<(bool Success, string? Error)> UpdateAsync(Guid enterpriseId, Guid vacancyId, RequestUpdateVacancyJson dto, CancellationToken ct = default);
        Task<IReadOnlyList<ResponseGetApplicationJson>> GetAllApplicationsByEnterpriseIdAsync(Guid enterpriseId);
        Task<ResponseGetApplicationJson> GetApplicationByCandidateId(Guid candidateId);
        Task<ResponseGetCandidateProfileJson> GetCandidateProfileInfoByCandidateId(Guid candidateId);
    }
}
