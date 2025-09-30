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
        public async Task<string> GetCandidateNameByIdAsync(Guid candidateId)
        {
            return (await _dbcontext.Candidates
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.CandidateIdentifier == candidateId)).Name;
        }
        public async Task<string> GetCandidateCourseNameByIdAsync(Guid candidateId)
        {
            return (await _dbcontext.Candidates
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.CandidateIdentifier == candidateId)).CourseName;
        }
        public async Task<string> GetCandidateEmailByUserIdAsync(Guid userId)
        {
            return (await _dbcontext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.UserIdentifier == userId)).Email;
        }
    }
}
