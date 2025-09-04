using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagio.Web.Components.Services.Enterprise
{
    public class EnterpriseService : BaseApiService, IEnterpriseService
    {
        public EnterpriseService(HttpClient http, IUserContext user)
            : base(http, user) { }

        public Task<List<ResponseVacancyListItem>> GetActiveAsync(Guid enterpriseId)
            => GetJsonAsync<List<ResponseVacancyListItem>>(
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
    }
}
