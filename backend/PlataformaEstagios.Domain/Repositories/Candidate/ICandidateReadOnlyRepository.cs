namespace PlataformaEstagios.Domain.Repositories.Candidate
{
    public interface ICandidateReadOnlyRepository
    {
        Task<Domain.Entities.Candidate> GetCandidateByIdAsync(Guid id, bool track, CancellationToken ct = default);
    }
}
