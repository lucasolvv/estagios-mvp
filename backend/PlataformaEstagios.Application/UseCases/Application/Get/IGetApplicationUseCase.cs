using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagios.Application.UseCases.Application.Get
{
    public interface IGetApplicationUseCase
    {
        Task<IReadOnlyList<ResponseGetApplicationJson>> GetRecentApplicationsByCandidateIdAsync(Guid candidateId);
    }
}
