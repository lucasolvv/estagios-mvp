using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagios.Application.UseCases.Candidate.Get
{
    public interface IGetCandidateUseCase
    {
        Task<ResponseGetCandidateProfileJson> GetCandidateByIdAsync(Guid id);
    }
}
