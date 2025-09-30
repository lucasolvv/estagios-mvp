namespace PlataformaEstagios.Domain.Repositories.Candidate
{
    public interface ICandidateReadOnlyRepository
    {
        Task<Domain.Entities.Candidate> GetCandidateByIdAsync(Guid id, bool track, CancellationToken ct = default);
        Task<string> GetCandidateNameByIdAsync(Guid candidateId);
        Task<string> GetCandidateCourseNameByIdAsync(Guid candidateId);
        Task<string> GetCandidateEmailByUserIdAsync(Guid userId);
    }
}
