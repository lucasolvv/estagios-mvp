using PlataformaEstagios.Communication.Requests;

namespace PlataformaEstagios.Application.UseCases.Application.Create
{
    public interface ICreateApplicationUseCase
    {
        Task CreateNewCandidateApplication(Guid vacancyId, Guid candidateId, CancellationToken ct);
    }
}
