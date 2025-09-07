using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagio.Web.Components.Services.Enterprise
{
    public class EnterpriseService : BaseApiService, IEnterpriseService
    {
        public EnterpriseService(HttpClient http, IUserContext user)
            : base(http, user) { }

        public async Task<List<ResponseVacancyListItem>> GetActiveAsync(Guid enterpriseId)
            => await GetJsonAsync<List<ResponseVacancyListItem>>(
                $"api/enterprises/{enterpriseId}/vacancies")!;

        public async Task<(bool Success, string? Error)> CreateAsync(Guid enterpriseId, RequestCreateVacancyJson dto, CancellationToken ct = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, $"api/enterprises/{enterpriseId}/vacancies")
            {
                Content = JsonContent.Create(dto)
            };

            var resp = await SendAsync(req, ct);
            if (resp.IsSuccessStatusCode) return (true, null);

            var msg = await resp.Content.ReadAsStringAsync(ct);
            return (false, string.IsNullOrWhiteSpace(msg) ? resp.StatusCode.ToString() : msg);
        }

        public async Task<ResponseGetVacancyJson> GetByIdAsync(Guid enterpriseId, Guid vacancyId)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, $"api/enterprises/{enterpriseId}/vacancies/{vacancyId}");
            var response = await SendAsync(req);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ResponseGetVacancyJson>();
            }

            return null;
        }

        public async Task<(bool Success, string? Error)> UpdateAsync(Guid enterpriseId, Guid vacancyId, RequestUpdateVacancyJson dto, CancellationToken ct = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Put, $"api/enterprises/{enterpriseId}/vacancies/{vacancyId}")
            {
                Content = JsonContent.Create(dto)
            };
            var resp = await SendAsync(req, ct);
            if (resp.IsSuccessStatusCode) return (true, null);
            var msg = await resp.Content.ReadAsStringAsync(ct);
            return (false, string.IsNullOrWhiteSpace(msg) ? resp.StatusCode.ToString() : msg);
        }
    }
}
