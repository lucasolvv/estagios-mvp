using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagio.Web.Components.Services.Enterprise
{
    public interface IEnterpriseService
    {
        Task<List<ResponseVacancyListItem>> GetActiveAsync(Guid enterpriseId);
    }
}
