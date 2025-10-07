using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;
using PlataformaEstagios.Domain.Enums;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace PlataformaEstagio.Web.Components.Services.Enterprise
{
    public class EnterpriseService : BaseApiService, IEnterpriseService
    {
        public EnterpriseService(HttpClient http, IUserContext user)
            : base(http, user) { }

        public async Task<List<ResponseVacancyListItem>> GetActiveAsync(Guid enterpriseId)
            => await GetJsonAsync<List<ResponseVacancyListItem>>(
                $"api/enterprises/{enterpriseId}/vacancies")!;

        public async Task<IReadOnlyList<ResponseGetApplicationJson>> GetAllApplicationsByEnterpriseIdAsync(Guid enterpriseId)
            => await GetJsonAsync<IReadOnlyList<ResponseGetApplicationJson>>(
                $"api/enterprises/{enterpriseId}/applications/all")!;

        public async Task<ResponseGetApplicationJson> GetApplicationByApplicationId(Guid applicationId)
            => await GetJsonAsync<ResponseGetApplicationJson>($"api/enterprises/candidatura/{applicationId}")!;

        public async Task<ResponseGetCandidateProfileJson> GetCandidateProfileInfoByCandidateId(Guid candidateId)
            => await GetJsonAsync<ResponseGetCandidateProfileJson>($"api/enterprises/candidato/{candidateId}")!;

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


        public async Task<(bool Success, string? Error)> UpdateApplicationStatus(Guid applicationId, ApplicationStatus status)
        {
            using var req = new HttpRequestMessage(HttpMethod.Put, $"api/enterprises/applications/{applicationId}/status")
            {
                Content = JsonContent.Create(new { Status = status.ToString() })
            };
            var resp = await SendAsync(req);
            if (resp.IsSuccessStatusCode) return (true, null);
            var msg = await resp.Content.ReadAsStringAsync();
            return (false, string.IsNullOrWhiteSpace(msg) ? resp.StatusCode.ToString() : msg);
        }

        public async Task<(bool Success, string? Error)> ScheduleInterviewAsync(Guid applicationId, RequestCreateScheduleInterviewJson body)
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, $"api/enterprises/applications/{applicationId}/interviews")
            { Content = JsonContent.Create(body) };

            var resp = await SendAsync(req);
            var text = await resp.Content.ReadAsStringAsync();

            return (resp.IsSuccessStatusCode, string.IsNullOrWhiteSpace(text) ? null : text);
        }

        public async Task<IReadOnlyList<ResponseGetInterviewItemJson>?> GetInterviewsByApplicationIdAsync(Guid applicationId)
        {
            using var req = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/enterprises/applications/{applicationId}/interviews");

            var response = await SendAsync(req);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<ResponseGetInterviewItemJson>>();

            return null;
        }

        public async Task<IReadOnlyList<ResponseGetInterviewItemJson>?> GetAllInterviewsByEnterpriseIdAsync(Guid enterpriseId)
        {
            using var req = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/enterprises/{enterpriseId}/interviews");
            
            var response = await SendAsync(req);
           
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<ResponseGetInterviewItemJson>>();
            return null;
        }
    }   


}
