using PlataformaEstagios.Communication.Requests;

namespace PlataformaEstagios.Application.UseCases.Candidate.UpdateProfile
{
    public interface IUpdateCandidateProfileUseCase
    {
        Task ExecuteAsync(Guid candidateId, RequestUpdateCandidateProfileJson request, CancellationToken ct = default);
    }
}
