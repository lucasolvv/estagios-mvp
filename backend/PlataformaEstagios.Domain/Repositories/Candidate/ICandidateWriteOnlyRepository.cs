namespace PlataformaEstagios.Domain.Repositories.Candidate
{
    public interface ICandidateWriteOnlyRepository
    {
        Task CreateCandidate(Entities.Candidate candidate);
    }
}
