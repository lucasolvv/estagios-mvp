using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagios.Application.UseCases.Application.Get
{
    public interface IGetApplicationUseCase
    {
        Task<IReadOnlyList<ResponseGetApplicationJson>> GetRecentApplicationsByCandidateIdAsync(Guid candidateId);
        Task<IReadOnlyList<ResponseGetApplicationJson>> GetRecentApplicationsByEnterpriseIdAsync(Guid enterpriseId);
        Task<ResponseGetApplicationJson> GetApplicationByCandidateIdAsync(Guid enterpriseId);

    }
}
