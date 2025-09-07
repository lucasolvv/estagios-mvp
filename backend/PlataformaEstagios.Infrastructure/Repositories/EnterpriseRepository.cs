using Microsoft.EntityFrameworkCore;
using PlataformaEstagios.Domain.Repositories.Enterprise;
using PlataformaEstagios.Infrastructure.DataAccess;

namespace PlataformaEstagios.Infrastructure.Repositories
{
    public class EnterpriseRepository : IEnterpriseWriteOnlyRepository, IEnterpriseReadOnlyRepository
    {
        private readonly AppDbContext _dbcontext;
        public EnterpriseRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task CreateEnterprise(Domain.Entities.Enterprise enterprise)
        {
            await _dbcontext.Enterprises.AddAsync(enterprise);
        }

        public async Task<string> GetEnterpriseNameByIdAsync(Guid enterpriseId)
        {
            return (await _dbcontext.Enterprises
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EnterpriseIdentifier == enterpriseId)).EnterpriseName;
        }

        public async Task<bool> ExistsAsync(Guid enterpriseId, CancellationToken ct)
        {
            return await _dbcontext.Enterprises
                .AnyAsync(e => e.EnterpriseIdentifier == enterpriseId, ct);
        }
    }
}
