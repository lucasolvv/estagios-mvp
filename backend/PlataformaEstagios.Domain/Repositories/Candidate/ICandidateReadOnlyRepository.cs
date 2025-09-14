namespace PlataformaEstagios.Domain.Repositories.Candidate
{
    public interface ICandidateReadOnlyRepository
    {
        Task<Entities.Candidate> GetCandidateByIdAsync(Guid id); 
    }
}
