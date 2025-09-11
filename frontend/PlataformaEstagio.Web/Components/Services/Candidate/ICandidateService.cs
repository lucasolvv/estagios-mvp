using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagio.Web.Components.Services.Candidate
{
    public interface ICandidateService
    {
        Task<IReadOnlyList<ResponseVacancyListItem>> GetOpenVacanciesAsync();
        Task<ResponseGetVacancyToApplicationJson> GetVacancyByIdAsync(Guid vacancyId, CancellationToken ct = default);
        Task<(bool success, string? error)> ApplyToVacancyAsync(RequestCreateApplicationJson request, CancellationToken ct = default);
        Task<IReadOnlyList<ResponseGetApplicationJson>> GetRecentApplicationsAsync(Guid candidateId);
    }
}
