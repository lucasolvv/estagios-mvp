using Microsoft.EntityFrameworkCore;
using PlataformaEstagios.Domain.Repositories.Application;
using PlataformaEstagios.Infrastructure.DataAccess;

namespace PlataformaEstagios.Infrastructure.Repositories
{
    public class ApplicationRepository : IApplicationWriteOnlyRepository, IApplicationReadOnlyRepository
    {
        private readonly AppDbContext _dbcontext;

        public ApplicationRepository(AppDbContext db)
        {
            _dbcontext = db;
        }

        public async Task AddAsync(Domain.Entities.Application application, CancellationToken ct)
        {
            await _dbcontext.Applications.AddAsync(application, ct);
        }

        public async Task<bool> ExistsAsync(Guid vacancyId, Guid candidateId)
        {
            return await _dbcontext.Applications
                .AnyAsync(a => a.VacancyId == vacancyId && a.CandidateIdentifier == candidateId);
        }
    }
}
