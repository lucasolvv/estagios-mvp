using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagio.Web.Components.Services.Candidate
{
    public class CandidateService : BaseApiService,  ICandidateService
    {
        public CandidateService(HttpClient http, IUserContext user)
            : base(http, user) { }

        public async Task<IReadOnlyList<ResponseVacancyListItem>> GetOpenVacanciesAsync() => await GetJsonAsync<List<ResponseVacancyListItem>>(
            "api/candidate/open-vacancies")!;

        //public async Task<ResponseGetVacancyJson> GetVacancyByIdAsync(Guid vacancyId, CancellationToken ct) => await GetJsonAsync<ResponseGetVacancyJson>(

    }
}
