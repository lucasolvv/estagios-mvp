using Microsoft.EntityFrameworkCore;
using PlataformaEstagios.Domain.Entities;
using PlataformaEstagios.Domain.Repositories.Interview;
using PlataformaEstagios.Infrastructure.DataAccess;

namespace PlataformaEstagios.Infrastructure.Repositories
{
    public class InterviewRepository : IInterviewWriteOnlyRepository, IInterviewReadOnlyRepository
    {

        private readonly AppDbContext _db;
        public InterviewRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Interview entity)
            => await _db.Interviews.AddAsync(entity);

        public async Task<bool> ExistsSameStartAsync(Guid applicationId, DateTimeOffset startAt)
            => await _db.Interviews.AnyAsync(i => i.ApplicationIdentifier == applicationId && i.StartAt == startAt);

        public async Task<IReadOnlyList<Interview>?> GetByApplicationIdAsync(Guid applicationId)
        {
            return await _db.Interviews
                .AsNoTracking()
                .Where(i => i.ApplicationIdentifier == applicationId)
                .ToListAsync();
        }
    }
}
