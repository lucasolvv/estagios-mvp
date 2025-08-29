using Microsoft.EntityFrameworkCore;
using PlataformaEstagios.Domain.Repositories.Vacancy;
namespace PlataformaEstagios.Infrastructure.Repositories
{
    public class VacancyRepository : IVacancyReadOnlyRepository, IVacancyWriteOnlyRepository
    {
        private readonly DataAccess.AppDbContext _context;
        public VacancyRepository(DataAccess.AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Domain.Entities.Vacancy entity, CancellationToken ct)
        {
            await _context.Vacancies.AddAsync(entity, ct);
        }
        public async Task<IReadOnlyList<Domain.Entities.Vacancy>> ListByEnterpriseAsync(Guid enterpriseId, CancellationToken ct)
        {
            return await _context.Vacancies
                .Where(v => v.EnterpriseIdentifier == enterpriseId && v.IsActive)
                .ToListAsync(ct);
        }
    }
}
