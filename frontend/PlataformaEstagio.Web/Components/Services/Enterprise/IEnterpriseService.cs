using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagio.Web.Components.Services.Enterprise
{
    public interface IEnterpriseService
    {
        Task<List<ResponseVacancyListItem>> GetActiveAsync(Guid enterpriseId);
        Task<(bool Success, string? Error)> CreateAsync(Guid enterpriseId, RequestCreateVacancyJson dto, CancellationToken ct = default);
    }
}
