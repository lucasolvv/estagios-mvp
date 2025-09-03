using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagio.Web.Components.Services.Enterprise
{
    public class EnterpriseService : IEnterpriseService
    {
        private readonly HttpClient _http;

        public EnterpriseService(HttpClient http) => _http = http;

        public async Task<List<ResponseVacancyListItem>> GetActiveAsync(Guid enterpriseId)
            => await _http.GetFromJsonAsync<List<ResponseVacancyListItem>>(
                $"api/enterprises/{enterpriseId}/vacancies") ?? [];

        //public async Task<EnterpriseHomeStatsResponse?> GetStatsAsync()
        //    => await _http.GetFromJsonAsync<EnterpriseHomeStatsResponse>(
        //        "api/enterprises/home/stats");

        //public async Task<List<RecentCandidateResponse>> GetRecentCandidatesAsync(int take = 10)
        //    => await _http.GetFromJsonAsync<List<RecentCandidateResponse>>(
        //        $"api/enterprises/home/recent-candidates?take={take}") ?? [];
    }
}
