using Microsoft.EntityFrameworkCore;
using PlataformaEstagios.Communication.Responses;
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

        // Infrastructure/Repositories/VacancyRepository.cs (exemplo LINQ)
        public async Task<IReadOnlyList<Domain.Entities.Vacancy>> GetActiveForEnterpriseAsync(
            Guid enterpriseId, CancellationToken ct = default)
        {
            return await _context.Vacancies
                .Where(v => v.EnterpriseIdentifier == enterpriseId && v.IsActive)
                .OrderByDescending(v => v.UpdatedAt) // ajuste seu critério
                .ToListAsync(ct);
        }

        //public async Task<EnterpriseHomeStatsResponse> GetStatsAsync(
        //    Guid enterpriseId, CancellationToken ct = default)
        //{
        //    var qVacancies = _context.Vacancies.Where(v => v.EnterpriseIdentifier == enterpriseId);

        //    var openCount = await qVacancies.CountAsync(v => v.IsActive, ct);
        //    var appsCount = await _context.Applications.CountAsync(a =>
        //                        qVacancies.Select(v => v.VacancyIdentifier)
        //                                  .Contains(a.VacancyId), ct);

        //    // Se ainda não há entrevistas, retorne 0
        //    var interviews = 0;

        //    // Ex.: taxa de preenchimento = encerradas / total (ajuste à sua regra)
        //    var total = await qVacancies.CountAsync(ct);
        //    var closed = await qVacancies.CountAsync(v => !v.IsActive, ct);
        //    var fillRate = total == 0 ? 0d : (double)closed / total;

        //    return new EnterpriseHomeStatsResponse(openCount, appsCount, interviews, fillRate);
        //}

        //public async Task<IReadOnlyList<RecentCandidateResponse>> GetRecentCandidatesAsync(
        //    Guid enterpriseId, int take, CancellationToken ct = default)
        //{
        //    return await _context.Applications
        //        .Where(a => a.Vacancy.EnterpriseIdentifier == enterpriseId)
        //        .OrderByDescending(a => a.CreatedAt) // ajuste ao seu campo real
        //        .Select(a => new RecentCandidateResponse(
        //            a.ApplicationIdentifier,
        //            a.Candidate.FullName,              // ajuste conforme seu modelo
        //            a.Candidate.Course,                // idem
        //            a.Vacancy.Title!,
        //            a.CreatedAt))
        //        .Take(take)
        //        .AsNoTracking()
        //        .ToListAsync(ct);
        //}

    }
}
