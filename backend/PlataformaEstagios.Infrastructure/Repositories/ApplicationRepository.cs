using Microsoft.EntityFrameworkCore;
using PlataformaEstagios.Domain.Enums;
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

        public async Task<IReadOnlyList<Domain.Entities.Application>> GetRecentApplicationsByCandidateIdAsync(Guid candidateId)
        {
            return await _dbcontext.Applications
                .AsNoTracking() // leitura pura
                .Where(a => a.CandidateIdentifier == candidateId)
                .Include(a => a.Vacancy)                // carrega a vaga
                .Include(a => a.Candidate)              // se o DTO usar dados do candidato
                .OrderByDescending(a => a.ApplicationDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Domain.Entities.Application>> GetRecentApplicationsByEnterpriseIdAsync(Guid enterpriseId)
        {
            return await _dbcontext.Applications
                .AsNoTracking()
                .Where(a => a.Vacancy.EnterpriseIdentifier == enterpriseId)
                .Include(a => a.Vacancy)   // se você vai ler a entidade Vacancy depois
                .Include(a => a.Candidate) // idem Candidate
                .OrderByDescending(a => a.ApplicationDate)
                .ToListAsync();
        }

        public async Task<Domain.Entities.Application?> GetApplicationByApplicationIdAsync(Guid applicationId)
        {
            return await _dbcontext.Applications
                .AsNoTracking()
                .Where(a => a.ApplicationIdentifier == applicationId)
                .Include(a => a.Vacancy)   // se você vai ler a entidade Vacancy depois
                .FirstOrDefaultAsync();
        }

        public async Task<Domain.Entities.Application?> GetByIdAsync(Guid applicationId, CancellationToken ct = default)
        {
            // Tracking habilitado (para update posterior)
            return await _dbcontext.Applications
                .Include(a => a.Vacancy)
                .Include(a => a.Candidate)
                .FirstOrDefaultAsync(a => a.ApplicationIdentifier == applicationId, ct);
        }

        public Task UpdateAsync(Domain.Entities.Application application, CancellationToken ct = default)
        {
            // Se já estiver tracked, o EF detecta mudanças; se não, marcamos como Modified
            _dbcontext.Entry(application).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        // Update só do status (eficiente). Requer EF Core 7+ por ExecuteUpdateAsync.
        public async Task<int> UpdateStatusAsync(Guid applicationId, ApplicationStatus newStatus, CancellationToken ct = default)
        {
            return await _dbcontext.Applications
                .Where(a => a.ApplicationIdentifier == applicationId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(a => a.Status, newStatus), ct); // << ajuste "a.Status" se o nome for diferente
        }
        public async Task<bool> BelongsToEnterpriseAsync(Guid applicationId, Guid enterpriseIdentifier)
        {
            return await _dbcontext.Applications
                .AnyAsync(a => a.ApplicationIdentifier == applicationId &&
                               a.Vacancy.EnterpriseIdentifier == enterpriseIdentifier);
        }
    }
}
