namespace PlataformaEstagio.Web.Components.Services.Candidate
{
    public interface ICandidateService
    {
        Task<IReadOnlyList<PlataformaEstagios.Communication.Responses.ResponseVacancyListItem>> GetOpenVacanciesAsync();
        //Task<PlataformaEstagios.Communication.Responses.ResponseGetVacancyJson> GetVacancyByIdAsync(Guid vacancyId, CancellationToken ct);
    }
}
