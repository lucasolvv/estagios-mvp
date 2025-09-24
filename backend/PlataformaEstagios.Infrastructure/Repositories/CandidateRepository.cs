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

        public async Task<Domain.Entities.Candidate?> GetCandidateByIdAsync(Guid id, bool track, CancellationToken ct = default)
        {
            var q = _dbcontext.Candidates
        .Include(c => c.Address)    // se precisar
        .Include(c => c.Applications);
            if (!track) q = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Domain.Entities.Candidate, 
                ICollection<Domain.Entities.Application>?>)q.AsNoTracking();
            return await q.FirstOrDefaultAsync(c => c.CandidateIdentifier == id, ct);
        }
    }
}
