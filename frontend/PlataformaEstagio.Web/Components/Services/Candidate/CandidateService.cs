using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagio.Web.Components.Services.Candidate
{
    public class CandidateService : BaseApiService,  ICandidateService
    {
        public CandidateService(HttpClient http, IUserContext user)
            : base(http, user) { }

        public async Task<IReadOnlyList<ResponseVacancyListItem>> GetOpenVacanciesAsync()
        {
            return await GetJsonAsync<List<ResponseVacancyListItem>>(
            "api/candidate/open-vacancies")!;
        }

        public async Task<ResponseGetVacancyToApplicationJson> GetVacancyByIdAsync(Guid vacancyId, CancellationToken ct = default)
        {
            return await GetJsonAsync<ResponseGetVacancyToApplicationJson>($"api/candidate/vacancies/{vacancyId}", ct)!;
        }

        public async Task<(bool success, string? error)> ApplyToVacancyAsync(RequestCreateApplicationJson request, CancellationToken ct = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, "api/candidate/application")
            {
                Content = JsonContent.Create(request)
            };

            var resp = await SendAsync(req);
            var text = await resp.Content.ReadAsStringAsync();

            return (resp.IsSuccessStatusCode, string.IsNullOrWhiteSpace(text) ? null : text);
        }

        public async Task<IReadOnlyList<ResponseGetApplicationJson>> GetRecentApplicationsAsync(Guid candidateId)
        {
            return await GetJsonAsync<List<ResponseGetApplicationJson>>(
                $"api/candidate/applications/{candidateId}")!;
        }

        public async Task<ResponseGetCandidateProfileJson> GetByIdAsync(Guid candidateId, CancellationToken ct = default)
        {
            // GET /api/candidate/{candidateId}
            return await GetJsonAsync<ResponseGetCandidateProfileJson>($"api/candidate/{candidateId}", ct)
                   ?? throw new InvalidOperationException("Não foi possível carregar os dados do candidato.");
        }

        public async Task<(bool success, string? error)> UpdateProfileAsync(
            Guid candidateId,
            RequestUpdateCandidateProfileJson dto,
            CancellationToken ct = default)
        {
            // PUT /api/candidate/{candidateId}/profile
            using var req = new HttpRequestMessage(HttpMethod.Put, $"api/candidate/{candidateId}/profile")
            {
                Content = JsonContent.Create(dto)
            };

            var resp = await SendAsync(req, ct);
            if (resp.IsSuccessStatusCode) return (true, null);

            return (false, await resp.Content.ReadAsStringAsync(ct));
        }

        public async Task<IReadOnlyList<ResponseGetInterviewItemJson>?> GetAllInterviewsByCandidateIdAsync(Guid candidateId)
        {
            using var req = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/candidate/{candidateId}/interviews");

            var response = await SendAsync(req);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<ResponseGetInterviewItemJson>>();
            return null;
        }

    }
}
