using Microsoft.EntityFrameworkCore;
using PlataformaEstagios.Domain.Repositories.Candidate;
using PlataformaEstagios.Infrastructure.DataAccess;

namespace PlataformaEstagios.Infrastructure.Repositories
{
    public class CandidateRepository : ICandidateWriteOnlyRepository, ICandidateReadOnlyRepository
    {
        private readonly AppDbContext _dbcontext;
        public CandidateRepository(AppDbContext db)
        {
            _dbcontext = db;
        }
        public async Task CreateCandidate(Domain.Entities.Candidate candidate)
        {
            await _dbcontext.Candidates.AddAsync(candidate);
        }

        public async Task<Domain.Entities.Candidate> GetCandidateByIdAsync(Guid id)
        {
            return await _dbcontext.Candidates.FirstOrDefaultAsync(c => c.CandidateIdentifier == id);
        }
    }
}
