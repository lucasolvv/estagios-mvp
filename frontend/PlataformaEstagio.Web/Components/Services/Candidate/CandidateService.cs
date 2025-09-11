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

            var resp = await SendAsync(req, ct);
            if (resp.IsSuccessStatusCode)
            {
                return (true, null);
            }

            return (false, await resp.Content.ReadAsStringAsync());
        }

        public async Task<IReadOnlyList<ResponseGetApplicationJson>> GetRecentApplicationsAsync(Guid candidateId)
        {
            return await GetJsonAsync<List<ResponseGetApplicationJson>>(
                $"api/candidate/applications/{candidateId}")!;
        }

    }
}
